import { GLFile } from '../types/listing/file';

// this utility function is responsible for ensuring we have an order assigned to files
export const sortGLFiles = (filesToSort:GLFile[]):GLFile[] => {
    // for now, ensure that the primary photo is 1 (re-factor if we implement user order capability)
    filesToSort.sort((a,b) => a.order && b.order && b.order > a.order ? 1 : -1);
    filesToSort.sort((a,b) => (b.primary) ? 1 : -1);
    // ensure all files have an order
    let order = 1;
    filesToSort.forEach((file:GLFile) => {
        file.order = order;
        order++;
    });
    return filesToSort;
}