  
export const environmentConfigs = {
    default: {
        adalConfig: {
            tenant: "0159e9d0-09a0-4edf-96ba-a3deea363c28",
            clientId: "1963a892-e605-4399-9fab-0d18ccb777f0",
            endpoints: {
                api: "1963a892-e605-4399-9fab-0d18ccb777f0"
            },
            cacheLocation: "localStorage"
        }
    },
    urlMatches : [
          {
            urls: ["dummy.cbrelistings.com"],
            adalConfig: {
                tenant: "0159e9d0-09a0-4edf-96ba-a3deea363c28",
                clientId: "729c089d-91f6-404e-80ad-02710292ee99",
                endpoints: {
                    api: "729c089d-91f6-404e-80ad-02710292ee99"
                },
                cacheLocation: "localStorage"
            }
        }
    ]
}
