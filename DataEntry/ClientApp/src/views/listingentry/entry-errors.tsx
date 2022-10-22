import React, {FC} from 'react'
import styled from 'styled-components';

const ErrorDisplay: FC = () => {

    const errors:string[] = [];
    
    if(errors !== undefined && errors.length > 0){
        return (
            <ErrorContainer>
                <ErrorHeader><ErrorSign>!</ErrorSign><ErrorHeaderTxt>Validation Errors!</ErrorHeaderTxt></ErrorHeader>
                <ErrorList>
                    {errors.map((error,idx) => {
                        return <ErrorListItem key={"err"+idx}>{error}</ErrorListItem>
                    })}
                </ErrorList>
            </ErrorContainer>
        )
    }
    return null;
};

const ErrorContainer = styled.div`
  
  background-color: #ffeee1;
  border-radius: 4px;
  color: darkred;
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  max-width:600px;
  margin: 8px auto 8px auto;
  padding:1em;
`;

const ErrorHeader = styled.div`
  
  text-align:center;
  font-size:20px;
  width:100%;
`;

const ErrorHeaderTxt = styled.div`
  display: inline-block;
`;

const ErrorList = styled.ul`
  list-style:none;
`

const ErrorListItem = styled.li`
 &::before{
   color: #fff;
   content: "\\2022";
   font-size:30px;
   padding-right:8px;
 }
`


const ErrorSign = styled.div`
  background-color: #cf8a90;
  border-radius: 50%;
  color: #fff;
  display: inline-block;
  line-height: 30px;
  margin-right: 8px;
  text-align: center;
  width: 30px;
`;

export default ErrorDisplay;