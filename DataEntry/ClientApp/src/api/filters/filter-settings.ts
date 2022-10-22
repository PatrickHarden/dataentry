import { FilterSetup, FilterType } from '../../types/listing/filters';

export const standardFilters:FilterSetup[] = [
    {
        uid: "viewall",
        label: "View All",
        selected: true,
        clearAll: true
    },
    {
        uid: "drafts",
        label: "Drafts",
        selected: false,
        allowMultiple: false,
        filter: {
            type: FilterType.PUBLISHING_STATUS.valueOf(),
            value: "Draft"
        }
    },
    {
        uid: "published",
        label: "Published",
        selected: false,
        allowMultiple: false,
        filter: {
            type: FilterType.PUBLISHING_STATUS.valueOf(),
            value: "Published"
        }
    },
    {
        uid: "pending",
        label: "Pending",
        selected: false,
        allowMultiple: false,
        filter: {
            type: FilterType.PUBLISHING_STATUS.valueOf(),
            value: "Pending"
        }
    }
];

export const bulkUploadFilter = {
    uid: "bulkuploads",
    label: "Bulk Uploads",
    selected: false,
    allowMultiple: true,
    filter: {
        type: FilterType.BULK_UPLOAD.valueOf(),
        value: "true"
    },
    category: "bulkUpload"
};

export const deletedFilter = {
    uid: "deleted",
    label: "Deleted",
    selected: false,
    allowMultiple: false,
    filter: {
        type: FilterType.DELETED.valueOf(),
        value: "true"
    }
}

export const importFromMIQFilter = {
    uid: "miq",
    label: "MIQ Imports",
    selected: false,
    allowMultiple: false,
    filter: {
        type: FilterType.MIQ.valueOf(),
        value: "true"
    }
}

