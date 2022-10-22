import React, { FC, useState } from 'react';
import { Document, Page, pdfjs } from 'react-pdf';
import styled from 'styled-components';

interface PDFPreviewProps {
    url: string,
    closeHandler?: () => void;
}

interface PageSizeProps {
    width: number,
    height: number
}

const PDFPreview: FC<PDFPreviewProps> = (props) => {

    pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

    const {url, closeHandler} = props;

    const [numPages, setNumPages] = useState<number | null>(null);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [loadError, setLoadError] = useState<boolean>(false);

    const pageSizeHeight:number = window.innerHeight * .80;

    const pageSize:PageSizeProps = {
        height: pageSizeHeight,
        width: Math.round(pageSizeHeight * .772727)
    };

    const documentLoaded = (data:any) => {
        setLoadError(false);
        setNumPages(data.numPages);
    }

    const documentLoadError = () => {
        setLoadError(true);
    }

    const closePreview = (e:any) => {
        e.preventDefault();
        if(closeHandler){
            closeHandler();
        }
    }

    const previousPage = () => {
        if(pageNumber > 1){
            const newPage = pageNumber - 1;
            setPageNumber(newPage);
        }
    }

    const nextPage = () => {
        if(numPages && pageNumber < numPages){
            const newPage = pageNumber + 1;
            setPageNumber(newPage);
        }
    }

    return (
        <PDFPreviewContainer>
            <PDFPreviewHeader {...pageSize}>
                <PDFPreviewHeaderText>{ numPages !== null && <PDFDownloadLink href={url}>Download PDF</PDFDownloadLink> }</PDFPreviewHeaderText>
                <PDFPreviewCloseButton onClick={closePreview}>X</PDFPreviewCloseButton>
            </PDFPreviewHeader>
            <PDFPreviewBody>
                {
                    loadError === true && <PDFPlaceholder {...pageSize}><ErrorText>Error Loading PDF</ErrorText></PDFPlaceholder>
                }
                { 
                    numPages === null && !loadError && <PDFPlaceholder {...pageSize}>Loading PDF Preview...</PDFPlaceholder>
                }
                <PDFDocument file={url} onLoadSuccess={documentLoaded} onLoadError={documentLoadError}>
                    
                    {
                        numPages !== null && (
                            <>
                                <PDFPage pageNumber={pageNumber} {...pageSize} />
                                <PDFPreviewFooter {...pageSize}>
                                    <PDFPreviewLeftArrow onClick={previousPage}>{ pageNumber && pageNumber > 1 && <div>&lt;</div>}</PDFPreviewLeftArrow> 
                                    <PDFPreviewFooterText>Page {pageNumber} of {numPages}</PDFPreviewFooterText>
                                    <PDFPreviewRightArrow onClick={nextPage}>{ pageNumber && numPages && numPages > 0 && pageNumber < numPages && <div>&gt;</div>}</PDFPreviewRightArrow>
                                </PDFPreviewFooter>
                            </>
                        )
                    }
                </PDFDocument>
            </PDFPreviewBody>
            
        </PDFPreviewContainer>
    );
}

const PDFPreviewContainer = styled.div`
    position:fixed;
    top:0;
    bottom:0;
    right:0;
    left:0;
    margin: 0 auto;
    background:rgba(0,0,0,0.6);
    z-index:2000;
    > span {
        color:#fff;
        font-size:35px;
        position:fixed;
        top:5px;
        right:15px;
        cursor:pointer
    }
    .react-pdf__Page__canvas {
        margin: 0 auto;
    }
`;

const PDFPreviewHeader = styled.div`
    border-bottom:solid 1px #333;
    padding: 20px 0 15px 0;
    background: #006A4D;
    color: white;
    width: ${(props: PageSizeProps) => (props.width ? props.width + 'px' : '612px')};
    margin: 0 auto;
`;

const PDFPreviewHeaderText = styled.div`
    margin-left: 10px;
    width: 100%;
    display: inline-block;
    width: 94%;
`;


const PDFDownloadLink = styled.a`
    margin-left: 20px;
    background-color: #00B2DD;
    color: #ffffff;
    border: 0px;
    padding: 0.7em 1.5em;
    cursor: pointer;
    display: inline-block;
    text-decoration: none;
`;

const PDFPreviewCloseButton = styled.a`
    color: white;
    font-size: 20px;
    font-weight: bold;
    display: inline-block;
    cursor: pointer;
    :hover {
        color: #FFFAFA;
    }
`;

const PDFPlaceholder = styled.div`
    width: ${(props: PageSizeProps) => (props.width ? props.width + 'px' : '612px')} !important;
    height: ${(props: PageSizeProps) => (props.height ? props.height + 'px' : '792px')} !important;;
    background: white;
    color: black;
    margin: 0 auto;
    text-align: center;
    font-size: 16px;
    font-weight: bold;
    display: flex;
    justify-content: center;
    align-items: center;
`;

const PDFDocument = styled(Document)``;

const PDFPage = styled(Page)`
    > canvas {
        width: ${(props: PageSizeProps) => (props.width ? props.width + 'px' : '612px')} !important; 
        height: ${(props: PageSizeProps) => (props.height ? props.height + 'px' : '792px')} !important;
    }
`;



const ErrorText = styled.div`
    color: darkred;
`;

const PDFPreviewBody = styled.div`
`;

const PDFPreviewFooter = styled.div`
    width: ${(props: PageSizeProps) => (props.width ? props.width + 'px' : '612px')};
    margin: 0 auto;
    background: #006A4D;
    color: white;
    padding: 10px 0 10px 0;
    text-align: center;
`;

const PDFPreviewFooterText = styled.div`
    display: inline-block;
    font-size: 16px;
`;

const PDFPreviewLeftArrow = styled.a`
    color: white;
    font-size: 20px;
    font-weight: bold;
    margin-right: 10px;
    cursor: pointer;
    display: inline-block;
    width: 50px;
`;

const PDFPreviewRightArrow = styled.a`
    color: white;
    font-size: 20px;
    font-weight: bold;
    margin-left: 10px;
    cursor: pointer;
    display: inline-block;
    width: 50px;
`;

export default PDFPreview;