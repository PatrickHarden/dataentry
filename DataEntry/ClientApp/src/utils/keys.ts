import { v1 } from 'uuid';

export const generateKey = ():string => {
    return v1().replace("-","");
}