import React, { FC } from 'react';
import StyledButton from '../styled-button/styled-button';
import styled from 'styled-components';
import { postBulkUploadFile } from '../../api/glAxios'
import UploadIcon from '../../assets/images/png/excelUploadIcon.png';
import { setAlertMessage } from '../../redux/actions/system/set-alert-message';
import { useDispatch } from 'react-redux';
import { AlertMessagingType, MainMessageType } from '../../types/state';
import { setMainMessage, clearMessage } from '../../redux/actions/system/set-main-message';
import { setConfirmDialog } from '../../redux/actions/system/set-confirm-dialog';
import { resetPagedListings } from '../../redux/actions/pagedListings/load-listings-paged';

// TODO: We really should not have axios logic within the component - we should try to ensure that this is in a reusable action/thunk for consistency
interface Props {
  sendAnalytics : () => void;
}

const BulkUploadButton: FC<Props> = (props: Props) => {
  const bulkUploadInputElement = React.createRef<HTMLInputElement>();
  const dispatch = useDispatch();
  const uploadFile = (files: FileList | null) => {


    if (!files) { return; };

    // TODO: Update redux state for loading component
    if (files[0]) {
      const acceptedFileTypes = [".xlsx", ".xls"];
      const fileType = files[0].name.substr(files[0].name.lastIndexOf('.'), files[0].name.length);
      let fileTypeAccepted = false;
      for (const acceptedFileType of acceptedFileTypes) {
        if (fileType.toLowerCase() === acceptedFileType.replace(/\s/g, '').toLowerCase()) {
          fileTypeAccepted = true;
          break;
        }
      }
      if (fileTypeAccepted) {
        // Post file to server
        // set loading message
        dispatch(setMainMessage({ show: true, type: MainMessageType.LOADING, message: "Uploading Listings..." }));
        postBulkUploadFile(files[0], (response: any) => {
            if (response.count) {
                clearMessage(dispatch);
                // reset the listings paging
                resetPagedListings(dispatch, true); 

                const successMsg: string = response.count + " record(s) imported successfully from " + response.fileName;
                dispatch(setAlertMessage({ show: true, message: successMsg, allowClose: true, type: AlertMessagingType.SUCCESS }));
            }
            else {
                clearMessage(dispatch);
            }

            if (response.errors && response.errors.length > 0) {
                dispatch(
                    setConfirmDialog(
                        {
                            show: true,
                            title: "Bulk Upload Error",
                            message: response.errors.join("\n\n"),
                            cancelTxt: "Close",
                            showConfirmButton: false,
                            showCopyButton: false,
                            scrollable: true
                        }));
            } else if (!response.count) {
                dispatch(setAlertMessage({ show: true, message: "FAIL: An error occurred processing the file. Please ensure that your file matches the bulk upload template.", allowClose: true, type: AlertMessagingType.ERROR }));
            }

        });

      }
      else {
        dispatch(setAlertMessage({ show: true, message: "FAIL: File Type Incorrect - Please upload correct file format (Excel Spreadsheet).", allowClose: true, type: AlertMessagingType.ERROR }));
      }

    }
  }

  const fireAnalytics = () => {
    props.sendAnalytics();
  }

    const clickUpload = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (bulkUploadInputElement.current != null) {
            bulkUploadInputElement.current.click();
        }
    }

  return (
      <BulkUploadForm>
        <StyledButton type="button" onClick={clickUpload}>
              <img src={UploadIcon} />
              <label> Bulk Upload</label>
              <BulkUploadInput
                  type="file"
                  id="bulk-upload-button"
                  name="bulk-upload-button"
                  ref={bulkUploadInputElement}
                  onChange={(x) => uploadFile(x.target.files)}
                  onClick={(x:any) => {
                      fireAnalytics();
                      x.target.value = null;
                      x.stopPropagation();
                  }}
                  accept=".xlsx,.xls" />
      </StyledButton>
    </BulkUploadForm>
  );
};

export default BulkUploadButton;

const BulkUploadForm = styled.form`
  float: right;
  margin-right: 8px;
  button {
      padding-left:42px;
      position:relative;
      label {cursor:pointer}
      img {
          height: 20px;
          width: 20px !important;
          position: absolute;
          left: 14px;
          top: 8px;
    }
}
`;

const BulkUploadInput = styled.input`
  display: none;
`;