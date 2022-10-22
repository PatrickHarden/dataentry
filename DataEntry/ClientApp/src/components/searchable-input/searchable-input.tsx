import React, { FC, useState, useCallback } from 'react';
import styled, { css } from 'styled-components';
import Autosuggest from 'react-autosuggest';
import FormFieldLabel from '../form-field-label/form-field-label';
import { AutoCompleteRequest, AutoCompleteResult } from '../../types/common/auto-complete';
import debounce from 'debounce';
import StyledButton from '../styled-button/styled-button';
import SearchIcon from '../../assets/images/png/icon-search-contacts.png';
import GreenSearchIcon from '../../assets/images/png/green-search-icon.png';
import { ScaleLoader } from 'react-spinners';

export interface SearchableInputProps {
  name?: string,
  label?: string,
  defaultValue?: any,
  placeholder?: string,
  error?: boolean,
  delayMS?: number,
  clearAfterSelect?: boolean,
  extraData?: string,
  useSearchData?: any,
  showSearchIcon?: boolean,
  noDataProps?: SearchableInputNoDataProps,
  showLoadingAnimation?: boolean,
  showGreenSearchIcon?: boolean,
  customErrorMessage?: string,
  disabled?: boolean,
  countryCode?: string,
  captureValue?(value: string): void,  // use only if we want to bubble up user input regardless of choice.  leave undefined if we require a choice.
  staticDataProvider?(request: AutoCompleteRequest): AutoCompleteResult[],    // use if you have a static non remote list to provide
  remoteDataProvider?(request: AutoCompleteRequest): Promise<AutoCompleteResult[]>, // use if you have a remote data provider (i.e., API) to provide
  autoCompleteFinish?(result: AutoCompleteResult): void
}

export interface StyleProps {
  error?: boolean,
  disabled?: boolean
}

interface SuggestionSelected {
  suggestion: AutoCompleteResult,
  suggestionValue: string,
  suggestionIndex: number,
  method: string
}

interface UserValue {
  newValue: string,
  method: string
}

export interface SearchableInputNoDataProps {
  showNoData?: boolean,
  noDataMessage?: string,
  showNoDataButton?: boolean,
  noDataButtonLabel?: string,
  noDataButtonCallback?(): void;
}

const SearchableInput: FC<SearchableInputProps> = (props) => {
  const { name, label, placeholder, captureValue, staticDataProvider, error, remoteDataProvider, showLoadingAnimation, delayMS, clearAfterSelect,
    autoCompleteFinish, extraData, useSearchData, showSearchIcon, noDataProps, showGreenSearchIcon, customErrorMessage, disabled, countryCode } = props;
  let { defaultValue } = props;

  let showNoData: boolean | undefined = false;
  let noDataMessage: string | undefined;
  let showNoDataButton: boolean | undefined = false;
  let noDataButtonLabel: string | undefined;
  let noDataButtonCallback: Function | undefined;

  if (noDataProps) {
    showNoData = noDataProps.showNoData;
    noDataMessage = noDataProps.noDataMessage;
    showNoDataButton = noDataProps.showNoDataButton;
    noDataButtonLabel = noDataProps.noDataButtonLabel;
    noDataButtonCallback = noDataProps.noDataButtonCallback;
  }

  const defaultDelayMS = 500;  // if no delayMS is passed in, 1/2 second is default on debounce
  const delay = delayMS ? delayMS : defaultDelayMS;
  const defaultPlaceholder: string = "Search for...";  // default placeholder if none passed in

  const [suggestions, setSuggestions] = useState<AutoCompleteResult[]>([]);
  const [noResults, setNoResults] = useState<boolean>(false);
  const [showLoad, setShowLoad] = useState<boolean>(false);
  const [showError, setShowError] = useState<string>('');

  if (!defaultValue) {
    defaultValue = "";
  }


  const [value, setValue] = useState(defaultValue);
  // force update equivalent
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState(undefined), []);

  const checkForResults = (request: AutoCompleteRequest, results: AutoCompleteResult[]) => {

    // if capture value is set, we bubble up the request value
    if (captureValue) {
      captureValue(request.value);
    }

    if (name) {
      const ele = document.getElementById(name);
      if (document.activeElement !== ele) {
        return;
      }
    }

    if (results && results.length === 0) {
      setNoResults(true);
    } else {
      setNoResults(false);
    }
  }

  const getSuggestionsRemote = (request: AutoCompleteRequest, passedValue: any) => {
    if (remoteDataProvider) {

      if (extraData) {
        request.extraData = extraData;
      }

      if (countryCode) {
        request.countryCodes = [passedValue]
      }

      setNoResults(false);
      setShowError('');
      if (showLoadingAnimation) {
        setShowLoad(true);
        remoteDataProvider(request).then(results => {
          checkForResults(request, results);
          setSuggestions(results);
          setShowLoad(false);
          forceUpdate();
        }).catch((err) => {
          setShowLoad(false);
          setShowError(customErrorMessage ? customErrorMessage : (typeof err === 'string' ? err : 'Error Searching...'));
          forceUpdate();
        });
      } else {
        remoteDataProvider(request).then(results => {
          checkForResults(request, results);
          setSuggestions(results);
          forceUpdate();
        });
      }
    }
  }

  const getSuggestionsStatic = (request: AutoCompleteRequest) => {
    if (staticDataProvider) {
      if (useSearchData) {
        request.useSearchData = useSearchData;
      }
      const results: AutoCompleteResult[] = staticDataProvider(request);
      checkForResults(request, results);
      setSuggestions(results);
      forceUpdate();
    }
  }

  // figure out which data provider to use : remote data provider will always be used over static if both are provided
  const suggestionsFunc = remoteDataProvider ? useCallback(debounce(getSuggestionsRemote, delay), []) : getSuggestionsStatic;

  const getSuggestionValue = (suggestion: any) => {
    return suggestion.name;
  }

  const renderSuggestion = (suggestion: any) => {
    return (
      <div>{suggestion.name}</div>
    );
  }

  const onSuggestionsClearRequested = () => {
    setSuggestions([]);
  };

  const onChange = (e: any, val: UserValue) => {
    setValue(val.newValue);
    if (showError && showError !== ''){
      setShowError('')
    }
  };

  const shouldRenderSuggestions = (val: string): boolean => {
    if (captureValue) {
      return true;
    } else {
      return val.trim().length > 1;
    }
  }

  const onSuggestionSelected = (e: any, suggestion: SuggestionSelected) => {
    if (autoCompleteFinish) {
      autoCompleteFinish(suggestion.suggestion);
    }
    if (clearAfterSelect) {
      setValue("");
    }
  }

  const inputProps = {
    'placeholder': placeholder ? placeholder : defaultPlaceholder,
    value,
    'onChange': onChange
  };

  const noDataClick = (e: React.MouseEvent) => {
    e.preventDefault();
    if (noDataButtonCallback) {
      noDataButtonCallback();
      setNoResults(false);
    }
  }

  const checkNoDataFocus = () => {
    // this is necessary to 
    if (noResults) {
      const ele = document.getElementById(name + "_dc");
      if (ele) {
        ele.focus();
      }
    }
  }

  const onBlur = (e: React.FocusEvent) => {
    const eleCheck = e.relatedTarget;
    const parent = document.getElementById(name + "_cont");
    if (parent && !checkIfChild(parent, eleCheck)) {
      setNoResults(false);
      if (captureValue) {
        captureValue(value);
      }
    }
  }

  const checkIfChild = (parent: any, child: any) => {
    if (!child || child === null) {
      return false;
    }
    let node = child.parentNode;
    while (node != null) {
      if (node === parent) {
        return true;
      }
      node = node.parentNode;
    }
    return false;
  }

  const renderInputComponent = (iProps: any) => (
    showGreenSearchIcon ? (
      <SearchInputContainer error={error} id="searchInputContainerGreen">
        <SearchInput id={name} {...iProps} />
        {showGreenSearchIcon && <GreenSearchIconContainer><img src={GreenSearchIcon} /></GreenSearchIconContainer>}
      </SearchInputContainer>
    ) : (
        <SearchInputContainer error={error}>
          <SearchInput id={name} {...iProps} />
          {showSearchIcon && !showGreenSearchIcon && <SearchIconContainer><img src={SearchIcon} /></SearchIconContainer>}
        </SearchInputContainer>
      )
  );

  const NoDataDropdown = () => {
    if (noResults && (showNoData || showNoDataButton)) {
      return (
        <DropdownContainer id={name + "_dc"} tabIndex={-1} onMouseOver={checkNoDataFocus}>
          <DropdownContent>
            {showNoData && <NoDataMessage>{noDataMessage ? noDataMessage : "No results found."}</NoDataMessage>}
            {showNoDataButton && <NoDataButtonContainer><StyledButton name={name + "-no-data-btn"} onClick={noDataClick}
              styledSpan={true} buttonStyle="2">{noDataButtonLabel ? noDataButtonLabel : "Add New"}</StyledButton></NoDataButtonContainer>}
          </DropdownContent>
        </DropdownContainer>
      );
    }
    return <></>;
  }

  return (
    <BaseInputContainer id={name + "_cont"} onBlur={onBlur} disabled={disabled}>
      <FormFieldLabel label={label} error={error} />
      <StyledAutoSuggestWrapper error={error} tabIndex={-1}>
        <Autosuggest
          suggestions={suggestions}
          onSuggestionsFetchRequested={(e) => suggestionsFunc(e, countryCode)}
          onSuggestionsClearRequested={onSuggestionsClearRequested}
          getSuggestionValue={getSuggestionValue}
          renderSuggestion={renderSuggestion}
          inputProps={inputProps}
          shouldRenderSuggestions={shouldRenderSuggestions}
          onSuggestionSelected={onSuggestionSelected}
          renderInputComponent={renderInputComponent} />
        <NoDataDropdown />
        {showLoad &&
          <LoaderContainer id={name + "_dc"} tabIndex={-1} onMouseOver={checkNoDataFocus}>
            <DropdownContent>
              <ScaleLoader
                color={'#00B2DD'}
                loading={true}
                height={40}
                width={4}
                margin={'4px'}
              />
            </DropdownContent>
          </LoaderContainer>
        }
        {showError !== '' &&
          <DropdownContainer id={name + "_dc"} tabIndex={-1} onMouseOver={checkNoDataFocus}>
            <DropdownContent>
              <NoDataMessage style={{color: 'darkred'}}>{showError}</NoDataMessage>
            </DropdownContent>
          </DropdownContainer>
        }
      </StyledAutoSuggestWrapper>
    </BaseInputContainer>
  );
}

const DropdownContainer = styled.div`
  position: relative;
`;

const LoaderContainer = styled.div`
    margin: 0 auto;
    text-align:center;
    position: relative;
    > div {
      padding:  115px 0;
    }
`;

const BaseInputContainer = styled.div`
  width:100%;
  opacity: ${(props: StyleProps) => props.disabled ? '.5' : 'auto' }; 
  pointer-events: ${(props: StyleProps) => props.disabled ? 'none' : 'inherit' };
  user-select: ${(props: StyleProps) => props.disabled ? 'none' : 'inherit' };
`;

const normalBorder = css`
1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const DropdownContent = styled.div`
  display: block;
  position: absolute;
  background: white;
  border: ${normalBorder};  
  border-top: none;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.1);
  width: 100%;
  z-index: 1000;
`;

const StyledAutoSuggestWrapper = styled.div`
  & .react-autosuggest__container {
    position: relative;
  } 
  & .react-autosuggest__input--focused {
    outline: none;
  }
  & .react-autosuggest__input--open {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
  & .react-autosuggest__suggestions-container {
    display: none;
  }
  & .react-autosuggest__suggestions-container--open {
    display: block;
    position: absolute;
    top: 40px;
    width: 100%;
    border: ${(props: StyleProps) => props.error ? errorBorder : normalBorder}; 
    background-color: #fff;
    font-family: Helvetica, sans-serif;
    font-weight: 300;
    font-size: 12px;
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
    z-index: 10000;
  }
  & .react-autosuggest__suggestions-list {
    margin: 0;
    padding: 0;
    list-style-type: none;
  }
  & .react-autosuggest__suggestion {
    cursor: pointer;
    color: hsl(0,0%,20%);
    font-size: 14px;
    padding: 10px 20px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
  }
  & .react-autosuggest__suggestion--highlighted {
    color: #fff; 
    background-color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : '#00b2dd')};
  }
`;

const SearchInputContainer = styled.div`
  border: ${(props: StyleProps) => props.error ? errorBorder : normalBorder}; 
  background-color: #fff;
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: flex;
  flex-direction: row;
  outline: 0;
  width:100%;
  :focus-within {
    outline: 1px solid ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
`;

const SearchInput = styled.input`
  border: none;
  padding: 1em;
  outline: 0;
  color: #666666; 
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  ::placeholder {color:#cccccc;}
  text-align: left;
  text-decoration: none;
  vertical-align: text-bottom;
  width: 100%;
`;

const SearchIconContainer = styled.div`
  color: #8E9A9D;
  text-align:right;
  filter: invert(1);
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 12px;
  margin-top:2px;
  cursor: pointer;
  padding: 10px 12px 5px 5px;
`;

const GreenSearchIconContainer = styled.div`
  color: #8E9A9D;
  text-align:right;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 12px;
  cursor: pointer;
  padding: 5px 0 5px 5px;
  position:absolute;
  right:5px;
  img {
    height:28px;
    margin-top:15px;
  }
`;

const NoDataMessage = styled.div`
  font-weight: normal !important;
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 14px;
  padding: 10px 0 10px 15px;
`;

const NoDataButtonContainer = styled.div`
  padding: 10px;
`;

export default SearchableInput;