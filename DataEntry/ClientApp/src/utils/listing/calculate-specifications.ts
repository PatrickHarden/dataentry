import { Space } from "../../types/listing/space";
import { Specifications } from '../../types/listing/specifications';

// minSpace, totalSpace, minLeasePrice, maxLeasePrice
export const calculateSpecifications = (specification: any, spaces: Space[], calcMinSpace?: boolean, calcTotalSpace?: boolean, calcMinLease?: boolean, calcMaxLease?: boolean, maxSalePrice?: boolean) => {
    const temp: Specifications = specification;
    if (spaces && spaces.length > 0){
        let maxPrice = 0;
        let minPrice = 0;
        let totalSpace = 0;
        let minSpace = 0;
        let totalSalePrice = 0;
        spaces.forEach((space: Space, index: number) => {
            if (space.specifications){
                totalSpace += space.specifications.totalSpace;
                totalSalePrice = space.specifications.salePrice;
                if (index === 0){
                    minPrice = space.specifications.maxPrice;
                    minSpace = space.specifications.totalSpace;
                    maxPrice = space.specifications.maxPrice;
                } else {
                    if (space.specifications.maxPrice < minPrice){
                        minPrice = space.specifications.maxPrice;
                    }
                    if (space.specifications.totalSpace < minSpace){
                        minSpace = space.specifications.totalSpace;
                    }
                    if (space.specifications.maxPrice > maxPrice){
                        maxPrice = space.specifications.maxPrice;
                    }
                }
            }
        })
        if (calcTotalSpace){
            temp.totalSpace = Math.round(totalSpace);
        }
        if (calcMinSpace){
            temp.minSpace = Math.round(minSpace);
        }
        if (calcMaxLease){
            temp.maxPrice = Math.round(maxPrice);
        }
        if (calcMinLease){
            temp.minPrice = Math.round(minPrice);
        }
        if (maxSalePrice){
            temp.salePrice = Math.round(totalSalePrice);
        }
        return temp;
    } else {
        return temp;
    }
}