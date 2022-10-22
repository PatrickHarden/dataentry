import { GLFile } from './file';
import { Address } from './address';

export interface Building{
    buildingName: string,
    address: Address,
    buildingDesc: string,
    locationDesc: String,
    photos: GLFile[],
    floorplans: GLFile[],
    brochures: GLFile[]
}