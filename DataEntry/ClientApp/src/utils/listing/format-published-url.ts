import { Listing } from "../../types/listing/listing";

export const formatExternalPublishedURL = (listing:Listing, externalPublishUrl:string) => {
    const listingAspects: string[] = ['sale', 'lease', 'salelease', 'investment'];
    const v1Aspects: string[] = ['isSale', 'isLetting', 'isSale&isLetting', 'isSale'];

    const aspectIndex: number = listingAspects.indexOf(listing.listingType);

    let updateUrl: string = externalPublishUrl;
    if (externalPublishUrl.includes('isLetting')) {
        updateUrl = externalPublishUrl.replace('isLetting', v1Aspects[aspectIndex]);
    }
    updateUrl = updateUrl.replace("/dataCentre/", "/datacenter/");
    return updateUrl;
}