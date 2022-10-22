import React, { FC } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { userSelector } from '../../redux/selectors/system/user-selector';
import { User } from '../../types/state';
import AdminHeader from './admin-header';
import AdminWatermark from './modules/admin-watermark/admin-watermark';
import TestingDashboard from './modules/testing-dashboard/testing-dashboard';
import { Col, Row, Grid } from 'react-styled-flexboxgrid';

const AdminContainer: FC = (props) => {

    const user:User = useSelector(userSelector);
    
    return (
        <div>
            { user && user.isAdmin &&  
                <div>
                    <AdminHeader/>
                    <Container>
                        <Module>
                            <TestingDashboard foo="bar"/>
                        </Module>
                        <Module>
                            <AdminWatermark/>
                        </Module>
                    </Container>
                </div>   
            }
            { !user || !user.isAdmin && <div>Sorry, these are not the droids you are looking for...</div>}
        </div>
    )
}

const Container = styled.div`
    display: flex
`;

const Module = styled.div`
    width: 50%;
`;

export default AdminContainer;

