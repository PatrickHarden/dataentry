import React, { FC, useState, useEffect, useRef, useCallback, useContext } from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { GLAnalyticsContext } from '../../../components/analytics/gl-analytics-context';

// images
import FilterIcon from '../../../assets/images/png/filterIcon.png';
import { FilterSetup } from '../../../types/listing/filters';

interface Props {
    setups: FilterSetup[],
    changeFilters: (filters: FilterSetup[], showMenu: boolean) => void
}

const ListingsFilter: FC<Props> = (props) => {

    const { setups, changeFilters } = props;
    const analytics = useContext(GLAnalyticsContext);

    const [showMenu, setShowMenu] = useState(false);
    const [, updateState] = useState();
    const forceUpdate = useCallback(() => updateState({}), []);
    
    let filterInfoDisplay = "";
    setups.forEach((setting: FilterSetup) => {
        if (setting.selected) {
            filterInfoDisplay = filterInfoDisplay + ' | ' + setting.label;
        }
    });

    const node = useRef(document.createElement("div"));

    const updateFilterMenu = () => {
        setShowMenu(!showMenu);
        setTimeout(() => { forceUpdate() }, 1);
    }

    const handleClick = (e: any) => {
        if (node.current.contains(e.target)) {
            return;
        }
        setShowMenu(false);
    };

    const selectFilter = (filter: FilterSetup, index: number) => {
        
        if(filter.label){
            analytics.fireEvent('filter', 'click', filter.label);
        }
        
        const changedFilters: FilterSetup[] = [...setups];
        changedFilters[0].selected = false; 

        // if the filter setup calls for a "clear all", then clear out all other filters (i.e, View All)
        if(filter.clearAll){
            changedFilters.forEach((changedFilter:FilterSetup) => {
                changedFilter.selected = false;
            });
            changedFilters[index].selected = true;
        }else if(filter.allowMultiple === false){
            // if we don't allow multiple of this "category", turn off all others in this "category"
            changedFilters.forEach((changedFilter:FilterSetup) => {
                if(changedFilter.category === filter.category){
                    changedFilter.selected = false;
                }
            });
            changedFilters[index].selected = true;
        }else if(filter.allowMultiple === true){
            changedFilters[index].selected = !changedFilters[index].selected;
        }
        changeFilters(changedFilters, showMenu);
    }

    useEffect(() => {
        document.addEventListener("mousedown", handleClick);
        return () => {
            document.removeEventListener("mousedown", handleClick);
        }
    }, []);

    return (
        <div ref={node} style={{ marginBottom: "20px" }}>
            <StyledListingsFilterButton onClick={updateFilterMenu}><img src={FilterIcon} />{filterInfoDisplay}</StyledListingsFilterButton>
            {showMenu && <StyledListingsFilterMenu>
                {setups.map((setup: FilterSetup, index: number) => {
                    if (setup.divider === true) {
                        return <hr key={"hr" + new Date().getTime()} />
                    } else {
                        return <span style={{ display: "flex" }} onClick={() => { selectFilter(setup, index) }} key={setup.label ? setup.label + new Date().getTime() : 'key_'+ index}>
                            <StyledCheckSpace> {(setup.selected) ? <FontAwesomeIcon icon={faCheck} /> : ''} </StyledCheckSpace><div style={{ flex: "1" }}>{setup.label}</div>
                        </span>
                    }
                })}
            </StyledListingsFilterMenu>}
        </div>
    );
};

export default ListingsFilter;

const StyledListingsFilterButton = styled.div`
    display: inline-block;
    padding:8px;
    background-color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
    cursor:pointer;
    color: #ffffff;    
`;

const StyledCheckSpace = styled.div`
    width:25px;
`;


const StyledListingsFilterMenu = styled.div`
    background-color: white;
    padding:10px;
    color:#00B2DD;
    border: 1px solid #00B2DD;
    z-index:15;
    position:absolute;
    min-width:200px;
    span {
        color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
        cursor:pointer;
        font-size: 14px;
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
        line-height: 20px; 
        position:relative;
        transition:.2s ease all;
        padding: 5px 10px 5px 5px;
        margin: 4px 0;
        transition:.2s ease all;
    }
    span:hover {
        background-color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
        color: white;
    }
`;