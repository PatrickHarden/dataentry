import React, { FC, useEffect, useState, useRef } from 'react';
import styled from 'styled-components';
import { Col, Row } from 'react-styled-flexboxgrid';
import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import { selectMIQExportResult } from '../../redux/actions/miq/set-miq-export-result';
import { useDispatch, useSelector } from 'react-redux';
import { miqCurrentRecordsSelector } from '../../redux/selectors/miq/miq-current-record-selector';
import { generateKey } from '../../utils/keys';
import RecordsDisplayView from './record-display/records-display-view';
import { Listing } from '../../types/listing/listing';
import { miqStatusSelector } from '../../redux/selectors/miq/miq-status-selector';
import { MoonLoader } from 'react-spinners';
import { setCancelAction } from '../../redux/actions/system/set-cancel-action';
import { RouteMatch } from '../../types/common/routes';

import { Config } from '../../types/config/config';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { saveListing, SaveType } from '../../redux/actions/listingEntry/save-listing';
import { TeamMember } from '../../types/team/teamMember';
import { authContext } from '../../adalConfig';
import defaultListing from '../../api/defaults/listing';

import SearchableInput, { SearchableInputNoDataProps } from '../../components/searchable-input/searchable-input';
import { getUsersForSearch } from '../../api/users/users';
import DeleteIcon from '../../assets/images/png/deleteIcon.png'
import cloneDeep from 'clone-deep';
import TeamMemberSingle from '../teams/team-member-single';
import StyledButton from '../../components/styled-button/styled-button';
import { MIQExportMessage } from '../../types/miq/miqExportMessaging';
import { miqExportMessageSelector } from '../../redux/selectors/miq/miq-export-message-selector';
import ExportResultView from './results/export-result-view';
import { push } from 'connected-react-router';
import { setSaveAction } from '../../redux/actions/system/set-save-action';
import { prepareRecordForCreateAndUnpublish, prepareRecordForAddEdit } from '../../redux/actions/miq/prepare-record-for-add-edit';
import { setMIQStatus } from '../../redux/actions/miq/set-miq-status';
import { setMIQRedirect } from '../../redux/actions/miq/set-miq-redirect';
import { miqRedirectSelector } from '../../redux/selectors/miq/miq-redirect-selector';
import { setConfirmDialog } from '../../redux/actions/system/set-confirm-dialog';
import { unpublishListing, UnpublishType } from '../../redux/actions/listingEntry/unpublish-listing';
import { AlertMessaging } from '../../types/state';
import { alertMessageSelector } from '../../redux/selectors/system/alert-message-selector';
import AlertMessage from '../common/messages/alert-message';
import { setMiqSpaces } from '../../redux/actions/miq/set-miq-spaces';
import { setMiqAddress } from '../../redux/actions/system/set-miq-address';

interface Props {
    match: RouteMatch
}

const MIQExportContainer: FC<Props> = (props) => {

    const dispatch = useDispatch();

    // selectors
    const currentRecords = useSelector(miqCurrentRecordsSelector);  // the loaded records (disabled in table(s))
    const status = useSelector(miqStatusSelector);  // loading indicator
    const config: Config = useSelector(configSelector);
    const exportMessage: MIQExportMessage = useSelector(miqExportMessageSelector);

    let id: string = "";
    // let currentRecord:Listing = {};
    const currentUser = authContext.getCachedUser();
    const assignedMembers: TeamMember[] = [];
    const assignees = { "users": assignedMembers };

    const [tempAssignees, setTempAssignees] = useState(cloneDeep(assignees));
    const defaultCurrentListing: Listing = defaultListing;
    const [currentRecord, setCurrentRecord] = useState(defaultCurrentListing);
    const [showAssignUI, setShowAssignUI] = useState(false);

    const marketIQRedirect = useSelector(miqRedirectSelector);

    const alertMessage: AlertMessaging = useSelector(alertMessageSelector);

    const { match } = props;
    if (match && match.params && match.params.id) {
        id = match.params.id;
    }

    const noDataProps: SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No users found...',
        showNoDataButton: false
    }

    const findRemoteDataProvider = (request: AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

        return new Promise((resolve, reject) => {
            resolve(getUsersForSearch(request));
        });
    }

    const getLatestUserIds = (): string[] => {

        const filterArray: string[] = tempAssignees.users.map((teamMember: TeamMember) => (teamMember.email));
        // include current user in filter
        filterArray.push(currentUser.userName);
        return filterArray;
    }

    const addUser = (suggestion: AutoCompleteResult) => {
        const suggesteduser = suggestion.value;
        const user: TeamMember = { teamMemberId: suggesteduser.teamMemberId, firstName: suggesteduser.firstName, lastName: suggesteduser.lastName, email: suggesteduser.email };
        // add user to assigned users
        const tAssignees = cloneDeep(tempAssignees);
        tAssignees.users.push(user);
        setTempAssignees(tAssignees);
    }

    const removeUser = (email: string) => {
        const tAssignees = cloneDeep(tempAssignees);
        const idx = tAssignees.users.findIndex((u: TeamMember) => u.email === email);
        tAssignees.users.splice(idx, 1);
        setTempAssignees(tempAssignees);
    }

    // attempt load once
    useEffect(() => {
        dispatch(setMIQRedirect(document.referrer));
        dispatch(selectMIQExportResult(id));
        dispatch(setMiqSpaces([]))
    }, []);

    // event handlers

    // user presses the create or edit button
    const handleUserAddEditAction = (record: Listing) => {
        if (!record.id || record.id < 1) {
            dispatch(setSaveAction({ goto: "/miq/create/" + id }));     // we will send creates back to the miq page (not sure about edits yet)
        }
        if (record && record.alternatePostalAddresses && record.alternatePostalAddresses.length > 0) {
            dispatch(setMiqAddress({ show: true }));
        } else {
            dispatch(setCancelAction({ goto: "/miq/create/" + id })); // ensure any cancel action in a subsequent page returns to this page)
            dispatch(prepareRecordForAddEdit(record, config.siteId));
        }
    }

    // user presses the assign button
    const handleUserAssignAction = (record: Listing) => {
        setCurrentRecord(record);
        setShowAssignUI(true);
        window.scrollTo(0, 0);
    }

    const cancelAssignChanges = () => {
        setTempAssignees(cloneDeep(assignees));
        setShowAssignUI(false);
    }

    const saveAssignChanges = () => {

        //   const newUser: TeamMember = { teamMemberId: member.name, firstName: member.firstName, lastName: member.lastName, email: member.name };

        const tempListing = cloneDeep(currentRecord);
        tempListing.owner = currentUser.userName;
        const ownerUser: TeamMember = { teamMemberId: "", firstName: currentUser.profile.given_name, lastName: currentUser.profile.family_name, email: currentUser.userName };
        tempListing.users = [];
        tempListing.users.push(ownerUser);

        tempAssignees.users.forEach((user: TeamMember) => {

            if (tempListing.users) {
                tempListing.users.push(user);
            }
        });

        tempListing.listingAssignment = {
            assignedBy: currentUser.userName,
            assignmentFlag: true,
            assignedDate: new Date()
        }

        setCurrentRecord(cloneDeep(tempListing));
        dispatch(saveListing(tempListing, SaveType.ASSIGN, config));

        dispatch(setMIQStatus({ loading: true, error: false, message: "" }));
        // setShowAssignUI(false);

        window.scrollTo(0, 0);

    }

    // turn off Assign UI when export is complete
    useEffect(() => {
        if (exportMessage && exportMessage.show) {
            setShowAssignUI(false);
        }
    }, [exportMessage]);

    const gotoMIQ = () => {
        if (marketIQRedirect && marketIQRedirect.length > 0) {
            window.location.href = marketIQRedirect;
        } else {
            window.location.href = "https://marketiq.cbre.com/";
        }
    }

    const gotoGL = () => {
        dispatch(push("/"));
    }

    useEffect(() => {
        setSaveAction({});  // reset save action
    }, []);

    // user presses the unpublish button
    const handleUserUnpublishAction = (record: Listing) => {
        dispatch(setConfirmDialog({
            show: true,
            title: "Confirm Remove Listings",
            message: "This will remove or unpublish the listing from the listings site.  Do you want to proceed?  You can always publish again in the future.",
            confirmTxt: "Unpublish",
            confirmFunc: () => handleUserConfirmUnpublishAction(record)
        }));
    }

    const handleUserConfirmUnpublishAction = (record: Listing) => {
        dispatch(setMIQStatus({ loading: true, error: false, message: "" }));
        if (!record.id || record.id < 1) {
            dispatch(prepareRecordForCreateAndUnpublish(record, undefined, config));
        }
        else {
            dispatch(unpublishListing(record, UnpublishType.MIQ_EXISTING));
        }
    }

    // removed the code below for now - design calls for it but doesn't make sense with edit actions available
    // <PageName><PageNameInnerContainer>Create Listing</PageNameInnerContainer></PageName>

    return (
        <PageContainer>
            {exportMessage && exportMessage.show &&
                <ResultViewContainer>
                    <ExportResultView exportMessage={exportMessage} users={tempAssignees.users} />
                    <ResultViewButtonBar>
                        <Left>
                            <NavigationButton onClick={gotoMIQ} >Back to Market IQ</NavigationButton>
                        </Left>
                        <Right>
                            <NavigationButton onClick={gotoGL} >Go to Global Listings</NavigationButton>
                        </Right>
                    </ResultViewButtonBar>
                </ResultViewContainer>
            }

            <GridContainer>
                {!(exportMessage && exportMessage.show) &&
                    <Row between="sm">
                        <Col sm={12}>
                            {status.loading && <LoadingContainer><MoonLoader color="#00B2DD" /></LoadingContainer>}
                            {!status.loading && currentRecords &&
                                <>
                                    {alertMessage && alertMessage.show && <AlertMessage alertMessage={alertMessage} />}
                                    <RecordsContainer>
                                        <RecordsDisplayView
                                            key={generateKey()}
                                            records={currentRecords}
                                            allowAssign={true}
                                            assignMode={showAssignUI}
                                            actionHandler={handleUserAddEditAction}
                                            assignHandler={handleUserAssignAction}
                                            allowUnpublish={config && config.featureFlags.unpublishFromMIQExport || false}
                                            unpublishHandler={handleUserUnpublishAction} />
                                    </RecordsContainer>
                                </>
                            }
                        </Col>
                    </Row>
                }
                <Row>
                    <Col sm={12}>
                        {(showAssignUI) &&
                            <div>
                                <TeamMembersHeader>Select Team Members</TeamMembersHeader>
                                <BaseTeamCard>
                                    <NameSection>
                                        <Row>
                                            <Col xs={12}>
                                                <SearchableInput key={"userSearch" + new Date().getTime()} name="userSearch" placeholder="Lookup and assign to members" remoteDataProvider={findRemoteDataProvider}
                                                    autoCompleteFinish={addUser} defaultValue="" clearAfterSelect={true} error={false} showSearchIcon={true} noDataProps={noDataProps} extraData={JSON.stringify(getLatestUserIds())} />
                                            </Col>
                                        </Row>

                                    </NameSection>
                                    {tempAssignees.users && tempAssignees.users.map((teamMember: TeamMember, index: number) => (
                                        <Row style={{ borderBottom: "1px solid #e8e8e8" }}>
                                            <Col xs={11} key={teamMember.teamMemberId + new Date().getTime() + index + ' ' + String((Math.floor((Math.random() * 100) + 1) / Math.floor((Math.random() * 100) + 1)) * Math.floor((Math.random() * 100) + 1))}>
                                                <TeamMemberSingle teamMember={teamMember} key={index} />
                                            </Col>
                                            <Col xs={1}>
                                                <span onClick={() => removeUser(teamMember.email)}><img style={{ width: "20px", marginTop: "18px", cursor: "pointer" }} src={DeleteIcon} /></span>
                                            </Col>
                                        </Row>
                                    ))}
                                </BaseTeamCard>
                                <ButtonBar>
                                    <ActionButton name={"cancelAssignBtn"} onClick={cancelAssignChanges} style={{ marginTop: '38px', textAlign: 'center' }} styledSpan={true}>Cancel</ActionButton>
                                    {(tempAssignees.users.length > 0) && <ActionButton name={"saveAssignBtn"} onClick={saveAssignChanges} style={{ marginTop: '38px', textAlign: 'center' }} styledSpan={true}>Assign</ActionButton>}
                                </ButtonBar>
                            </div>
                        }
                    </Col>
                </Row>
            </GridContainer>
        </PageContainer>
    );
};

const PageContainer = styled.div`
    background: #fdfdfd;
    width:100%;
    min-height: 100vh;
    font-family: Futura Md BT Bold;
`;

const RecordsContainer = styled.div`
    margin-top: 90px;
`;

const LoadingContainer = styled.div`
    display: flex;
    width: 100%;
    height: 200px;
    align-items: center;
    justify-content: center;
`;

const GridContainer = styled.div`
    > div {
        max-width:100%;
    }
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
`;

const TeamMembersHeader = styled.div`
    color: #666;
    font-size: 24px;
    font-weight: 800;
    margin-bottom: 20px;
`;

const BaseTeamCard = styled.div`
    width: auto;
    margin: auto;
    padding: 15px;
    overflow:visible;
    position:relative;
    margin-bottom: 0;
    border: 1px solid #ddd;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
            transition: all 0.3s cubic-bezier(.25,.8,.25,1);
            /* &:hover {
                box-shadow: 0 7px 14px rgba(204,204,204,0.20), 0 5px 5px rgba(204,204,204,0.17);
            } */
            background-color: white;
`;

const NameSection = styled.div`
    border-bottom: 1px solid #ddd;
    padding-bottom: 25px;
    #userSearch_cont {
        position:relative;
        top: 8px;
        #userSearch {
            padding: 1.08em;
        }
        #react-autowhatever-1 {
            overflow-y: auto;
            max-height: 50vh;
        }
    }
`;

const ResultViewContainer = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    font-size:20px
`;

const ResultViewButtonBar = styled.div`
    display: flex;
    justify-content: space-between;
`;

const Left = styled.div`
    display: inline-block;
`;

const Right = styled.div`
    display: inline-block;
`;

const ActionButton = styled(StyledButton)`
    font-family: 'Futura Md BT Bold',helvetica,arial,sans-serif;
    background-color: #006A4D;
    border-radius: 4px;
    margin-left: 8px;
`;

const NavigationButton = styled(StyledButton)`
    border-radius: 0;
    background: #006A4D;
    text-transform: uppercase;
`;

const ButtonBar = styled.div`
    text-align: right;
    margin-top: -20px;
`;

export default MIQExportContainer;