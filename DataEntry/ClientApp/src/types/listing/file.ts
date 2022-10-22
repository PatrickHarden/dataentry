export interface GLFile {
    id?: number,
    url: string,
    displayText: string,
    active: boolean | null,
    primary:boolean | null,
    order?: number | null,
    watermark?: boolean | null,
    watermarkProcessStatus?: WatermarkProcessStatus,
    userOverride?: boolean | null,
    errorDisplay?: string | null,
    loadingMsg?: string | null,
    base64String?: string | null
}

export enum WatermarkProcessStatus {
    SENDING_FILE_TO_SERVER = -1,
    READY_TO_PROCESS = 0,
    PROCESSING = 1,
    NO_WATERMARK = 2,
    WATERMARK = 3,
    WATERMARK_ERROR = 4,
    NOT_PROCESSED = 5,
    CRE_WATERMARK = 6
}