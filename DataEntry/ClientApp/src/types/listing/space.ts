import { GLFile } from './file';
import { Specifications } from './specifications';
import { MultiLangString } from './multi-lang-string';
import { SizeType } from '../../views/listingentry/sizesAndMeasurements/sizes-and-measurements';

export interface Space {
    id: number,
    miqId?: any,
    availableFrom?: string | null,
    name: MultiLangString[],
    nameSingle: string,
    spaceDescription: MultiLangString[],
    spaceDescriptionSingle: string,
    specifications: Specifications,
    photos: GLFile[],
    floorplans: GLFile[],
    brochures: GLFile[],
    video: string,
    walkThrough: string,
    spaceSizes: SizeType[]
}