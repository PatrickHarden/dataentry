import React, { useState, FC, useMemo } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import iconSuccess from '../../assets/images/png/icon-success.png';
import { Tabs, TabList, Tab, TabPanel } from 'react-tabs';

export interface FormTabbedTextAreaProps {
  name?: string,
  label?: string,
  placeholder?: string,
  defaultValue?: string,
  error?: boolean,
  theme?: any,
  appendToId?: string,
  data?: any,
  tabsettings?: any,
  index?: number,
  updateTabData?(e: any): void,
  updateIndexedTabData?(index: any, e: any): void,
}

export interface StyleErrorProps {
  error?: boolean
}

const FormTabbedTextArea: FC<FormTabbedTextAreaProps> = (props) => {

  const { name, label, error, appendToId, data, tabsettings, index, updateTabData, updateIndexedTabData } = props;

  const [langsettings, setlangsettings] = useState(tabsettings);
  const [langdata, setlangdata] = useState(data);
  const [currentIdx, setCurrentIdx] = useState<number>(0);

  let currentValue: string = "";

  const initial = Array.from({ length: langsettings.length }, (v, idx) => idx).map(idx => {
    let arrayLangData = [];

    let custom: any = {
      value: ``,
      text: ``
    };

    if (langdata && langdata !== null && langdata.length !== 0) {
      if (Array.isArray(langdata)) {
        arrayLangData = langdata;
      } else {
        arrayLangData.push(langdata);
      }
    }

    if (arrayLangData.length > 0) {
      const itemData = arrayLangData.filter((d: any) => {
        return d.cultureCode === langsettings[idx].cultureCode;
      });
      if (itemData.length > 0) {
        custom = {...custom, ...itemData[0], value: itemData[0].text};
      }
    }

    custom.id = `${idx}`;
    custom.placeholder = `${langsettings[idx].placeholder ? langsettings[idx].placeholder : ""}`;
    custom.cultureCode = `${langsettings[idx].cultureCode ? langsettings[idx].cultureCode : idx}`;
    custom.lang = `${langsettings[idx].lang ? langsettings[idx].lang : idx}`;

    return custom;
  });

  const [state, setState] = useState({ tabdata: initial });

  let id: string | undefined = name;
  if (appendToId && appendToId.length > 0) {
    id = id += "_" + appendToId;

  }

  const updateValue = (idx: number, value: any) => {

    const tempState: any = state;

    if (tempState.tabdata[idx]) {
      tempState.tabdata[idx].value = value;
    }

    setState(Object.assign({}, tempState));
    handleChange(tempState.tabdata);
  }

  const handleChange = (changedValues: any) => {

    // before we bubble data up, get it back into the form that it was sent in (array of MultiLanguageItems)
    let valueArray: any = [];
    if (changedValues.length > 0) {
      valueArray = Array.from({ length: changedValues.length }, (v, idx) => idx).map((idx: any) => {
        // if (changedValues[idx] && changedValues[idx].value !== "") {
        if (changedValues[idx]) {
          const val: any = {
            ...changedValues[idx],
            text: `${changedValues[idx].value ? changedValues[idx].value : ""}`
          };
          return val;
        } else {
          return null;
        }
      });
    }
    // remove null values
    const removeNullVals = valueArray.filter((d: any) => {
      return d !== null;
    });

    if (updateTabData !== undefined) {
      // props.updateTabData(removeNullVals);
      updateTabData(removeNullVals);
    }
    if (updateIndexedTabData !== undefined) {
      updateIndexedTabData(index, removeNullVals);
    }
  }

  const selectNewTab = (idx: number) => {

    if (currentValue.length > 0) {
      const tempState: any = Object.assign({}, state);
      if (tempState.tabdata[currentIdx]) {
        tempState.tabdata[currentIdx].value = currentValue;
      }
      setState(tempState);
      handleChange(tempState.tabdata);
    }

    setCurrentIdx(idx);
  }


  // handle the blur event and use the callback function to set our value upwards
  const handleBlur = (e: React.FormEvent<HTMLTextAreaElement>) => {
    
    updateValue(Number(e.currentTarget.id), e.currentTarget.value);
  
  }

  const changeValue = (e: React.FormEvent<HTMLTextAreaElement>) => {

    currentValue = e.currentTarget.value;

  }

  const getAccKey = (item: any, idx: number) => {
    let extraKey: string = new Date().getTime().toString();
    if (item.id) {
      extraKey = item.id;
    }
    return "acc_" + idx + "_" + extraKey;
  }

  const TabsMemo = useMemo(() =>
    state.tabdata.map((item: any) => (
      <STab><TabText>{item.lang}</TabText>{item.value && typeof item.value === "string" && item.value.trim().length > 0 && <TabIcon src={iconSuccess}/>}</STab>
    )), [state]);

  const generateTabPanels = () => {
    return state.tabdata.map((item: any, idx: number) => (
      <STabPanel><BaseFormTabbedTextArea key={getAccKey(item, idx)} id={item.id} error={error} placeholder={item.placeholder} name={item.value} defaultValue={item.value} onBlur={handleBlur} onChange={changeValue} /></STabPanel>
    ));
  }

  return (
    <BaseInputContainer>
      <FormFieldLabel label={label} error={error} />
      <STabs
        selectedTabClassName='is-selected'
        selectedTabPanelClassName='is-selected'
        onSelect={tabindex => selectNewTab(tabindex)}
      >
        <STabList>
          <>{TabsMemo}</>
        </STabList>
        <>{generateTabPanels()}</>
      </STabs>

    </BaseInputContainer>
  );
};

const STabs = styled(Tabs)`
  font-size: 12px;
`;

const STabList = styled(TabList)`
  list-style-type: none;
  padding: 0px 4px 4px 0px;
  display: flex;
  background-color: #e9e9e9;
  margin: 0;
`;

const STabPanel = styled(TabPanel)`
  display: none;
  /* min-height: 40vh; */
  border: 1px solid #d3d3d3;
  background-color: white;
  padding: 15px;
  margin-top: -5px;

  &.is-selected {
    display: block;
  }
`;

const STab = styled(Tab)`
  margin-right: -1px;
  border: 1px solid #e9e9e9;
  border-bottom: 1px solid #d3d3d3;
  padding: 10px;
  user-select: none;
  cursor: pointer;
  color: 1px solid #d3d3d3;
  display: inline-flex;

  &.is-selected {
    // color: white;
    // background: black;
    border: 1px solid #d3d3d3;
    color: 1px solid #c6c6c6;
    border-bottom: 1px solid white;
    background-color: white;
    cursor: arrow;
  }

  &:focus {
    outline: none;
    box-shadow: 0 0 0 2px rgba(0, 0, 255, .5)
  }
`;



const BaseInputContainer = styled.div`
  width:100%;
`;

const normalBorder = css`
1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const BaseFormTabbedTextArea = styled.textarea`
  color: #666666; 
  border: ${(props: StyleErrorProps) => props.error ? errorBorder : normalBorder}; 
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: block;
  flex-grow: 1;
  outline: 0; 
  padding: 1em; 
  ::placeholder {color:#dadada};
  resize: vertical;
  text-align: left;
  text-decoration: none;
  width:100%;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
  :focus {
    border-color: ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
  font-size: ${props => (props.theme.formSize ? props.theme.formSize.input : '14px')}; 
`;

const TabText = styled.div``;

const TabIcon = styled.img`
  width: 15px;
  height: 15px;
  margin-left: 5px;
`;

export default FormTabbedTextArea;