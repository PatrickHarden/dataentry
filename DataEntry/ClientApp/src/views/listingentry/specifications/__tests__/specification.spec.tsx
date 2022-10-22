import React from 'react';
import { mount } from 'enzyme';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

import SpecificationsView from '../specifications';

describe('views', () => {

  describe('SpecificationsView', () => {
    let props: any;
    let wrapper: any;
    let store: any;

    // state.router.location.pathname;
    // {pathname: '/', search: '', hash: '', state: undefined, key: 'nu7r1t', …}
    const initialState = {
      system: {
        configDetails: {
          loaded: true,
          config: {}
        }
      },
      entry: {
        spaces: []
      },
      router: {
        location: {
          pathname: '/'
        }
      }
    };
    const mockStore = configureStore();

    beforeEach(() => {
      props = {
        listing: {
          specifications: {
            contactBrokerForPrice: true
          }
        }
      }
      store = mockStore(initialState);
    });

    describe('common tests', () => {

      let commonTestFields = {};

      beforeEach(() => {
        props.listing.propertyType = 'retail';
        props.listing.listingType = 'lease';

        commonTestFields = {
          "values": ['industrial','retail','office'],
          "lease": {
              "leaseType": {
                  "show": true,
                  "properties": {
                      "name": "leaseType",
                      "label": "Lease Type",
                      "prompt": "Select Lease Type...",
                      "options": [
                          { "label": "Direct", "value": "Direct", "order": 1 },
                          { "label": "Indirect", "value": "Indirect", "order": 2 }
                      ]
                  },
                  "grid": {
                      "colSize": 12
                  }
              },
              "measure": {
                  "show": true,
                  "properties": {
                      "name": "measure",
                      "label": "Area Measure",
                      "prompt": "Select Measurement...",
                      "options": [
                          { "label": "SM  (Square Meter)", "value": "sm", "order": 1 },
                          { "label": "SF  (Square Feet)", "value": "sf", "order": 2 },
                          { "label": "Acre", "value": "acre", "order": 3 }
                      ] 
                  },
                  "grid": {
                      "colSize": 6
                  }
              },
              "leaseTerm": {
                  "show": true,
                  "properties": {
                      "name": "leaseTerm",
                      "label": "Lease Term",
                      "prompt": "Select Lease Term...",
                      "options": [
                          { "label": "Monthly", "value": "monthly", "order": 1 },
                          { "label": "Quarterly", "value": "quarterly", "order": 2 },
                          { "label": "Yearly", "value": "yearly", "order": 3 }
                      ]
                  },
                  "grid": {
                      "colSize": 6
                  }
              },
              "minSpace": {
                  "show": true,
                  "properties": {
                      "name": "minSpace",
                      "label": "Minimum Space",
                      "numericOnly": true,
                      "acceptDecimals": false,
                      "serverDataType": "INTEGER"
                  }
              },
              "maxSpace": {
                  "show": true,
                  "properties": {
                      "name": "maxSpace",
                      "label": "Total Space Available",
                      "numericOnly": true,
                      "acceptDecimals": false,
                      "serverDataType": "INTEGER"
                  },
                  "grid": {
                      "colSize": 6
                  }
              },
              "minPrice": {
                  "show": true,
                  "properties": {
                      "name": "minPrice",
                      "label": "Minimum Price",
                      "numericOnly": true,
                      "acceptDecimals": true,
                      "serverDataType": "DECIMAL_CURRENCY"
                  },
                  "grid": {
                      "colSize": 6
                  }
              },
              "maxPrice": {
                  "show": true,
                  "properties": {
                      "name": "maxPrice",
                      "label": "Maximum Price",
                      "numericOnly": true,
                      "acceptDecimals": true,
                      "serverDataType": "DECIMAL_CURRENCY"
                  },
                  "grid": {
                      "colSize": 6
                  }
              },
              "contactBrokerForPrice": {
                  "show": true,
                  "properties": {
                      "name": "contactBrokerForPrice",
                      "label": "Contact Broker For Pricing"
                  },
                  "grid": {
                      "colSize": 12
                  }
              }
          }
        }

        const commonTestsStore = {
          system: {
            configDetails: {
              loaded: true,
              config: {
                specifications: {
                  normalFields: commonTestFields
                }
              }
            }
          },
          entry: {
            spaces: []
          },
          router: {
            location: {
              pathname: '/'
            }
          }
        };
        store = mockStore(commonTestsStore);
      });

      // check the default currency icon (should be $ if not specified)
      it("default currency icon $", () => {
        wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
        const minPrice = wrapper.find('[name="minPrice"]');
        expect(minPrice.exists('[prefix$="$"]')).toBeTruthy();
      });

      // when specified, it should reflect the specifications
      // it("currency icon when specified should be ₿", () => {
      //   const iconCheckStore = {
      //     system: {
      //       configDetails: {
      //         loaded: true,
      //         config: {
      //           specifications: {
      //             currency: "₿",
      //             normalFields: commonTestFields
      //           }
      //         }
      //       }
      //     }
      //   };
      //   store = mockStore(iconCheckStore);
      //   wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
      //   const minPrice = wrapper.find('[name="minPrice"]');
      //   expect(minPrice.exists('[prefix$="₿"]')).toBeTruthy();
      // }); 
    });

    describe('Normal Fields (Retail, Industrial, Office) - Listing Type: Sale Tests', () => {

      // state.entry.spaces
        beforeEach(() => {
          props.listing.propertyType = 'retail';
          props.listing.listingType = 'sale';
          const normalSaleStore = {
            system: {
              configDetails: {
                loaded: true,
                config: {
                  specifications: {
                    normalFields: {
                      "values": ["retail","industrial","office"],
                      "sale": {
                          "leaseType": {
                              "show": false
                          },
                          "measure": {
                              "show": true,
                              "properties": {
                                  "name": "measure",
                                  "label": "Area Measure",
                                  "prompt": "Select Measurement...",
                                  "options": [
                                      { "label": "SM  (Square Meter)", "value": "sm", "order": 1 },
                                      { "label": "SF  (Square Feet)", "value": "sf", "order": 2 },
                                      { "label": "Acre", "value": "acre", "order": 3 }
                                  ] 
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "leaseTerm": {
                              "show": false
                          },
                          "minSpace": {
                              "show": false
                          },
                          "maxSpace": {
                              "show": true,
                              "properties": {
                                  "name": "maxSpace",
                                  "label": "Total Space Available",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minPrice": {
                              "show": true,
                              "properties": {
                                  "name": "minPrice",
                                  "label": "Price",
                                  "numericOnly": true,
                                  "acceptDecimals": true,
                                  "serverDataType": "DECIMAL_CURRENCY"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          },
                          "maxPrice": {
                              "show": false
                          },
                          "contactBrokerForPrice": {
                              "show": true,
                              "properties": {
                                  "name": "contactBrokerForPrice",
                                  "label": "Contact Broker For Pricing"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          }
                      }
                    }
                  }
                }
              }
            },
            entry: {
              spaces: []
            },
            router: {
              location: {
                pathname: '/'
              }
            }
          };
          store = mockStore(normalSaleStore);
        });

        it("Retail:Sale - lease type should not be shown", () => {
            wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
            const minPrice = wrapper.find('[name="leaseType"]');
            expect(minPrice.exists('[name="leaseType"]')).toBeFalsy();
        });

        it("Retail:Sale - measure should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const measure = wrapper.find('[name="measure"]');
          expect(measure.exists('[name="measure"]')).toBeTruthy();
        });

        it("Retail:Sale - lease term should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const leaseTerm = wrapper.find('[name="leaseTerm"]');
          expect(leaseTerm.exists('[name="leaseTerm"]')).toBeFalsy();
        });

        it("Retail:Sale - min space should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minSpace = wrapper.find('[name="minSpace"]');
          expect(minSpace.exists('[name="minSpace"]')).toBeFalsy();
        });

        it("Retail:Sale - max space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const maxSpace = wrapper.find('[name="maxSpace"]');
          expect(maxSpace.exists('[name="maxSpace"]')).toBeTruthy();
        });

        it("Retail:Sale - min price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[name="minPrice"]')).toBeTruthy();
        });

        it("Retail:Sale - min price label should be Price", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[label="Price"]')).toBeTruthy();
        });

        it("Retail:Sale - max price should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="maxPrice"]');
          expect(minPrice.exists('[name="maxPrice"]')).toBeFalsy();
        });

        it("Retail:Sale - contact broker for price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="contactBrokerForPrice"]');
          expect(minPrice.exists('[name="contactBrokerForPrice"]')).toBeTruthy();
        });
    });

    describe('Normal Fields (Retail, Industrial, Office) - Listing Type: Lease and Sale/Lease Tests', () => {

        beforeEach(() => {
          props.listing.propertyType = 'retail';
          props.listing.listingType = 'lease';
          const normalLeaseStore = {
            system: {
              configDetails: {
                loaded: true,
                config: {
                  specifications: {
                    normalFields: {
                      "values": ['industrial','retail','office'],
                      "lease": {
                          "leaseType": {
                              "show": true,
                              "properties": {
                                  "name": "leaseType",
                                  "label": "Lease Type",
                                  "prompt": "Select Lease Type...",
                                  "options": [
                                      { "label": "Direct", "value": "Direct", "order": 1 },
                                      { "label": "Indirect", "value": "Indirect", "order": 2 }
                                  ]
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          },
                          "measure": {
                              "show": true,
                              "properties": {
                                  "name": "measure",
                                  "label": "Area Measure",
                                  "prompt": "Select Measurement...",
                                  "options": [
                                      { "label": "SM  (Square Meter)", "value": "sm", "order": 1 },
                                      { "label": "SF  (Square Feet)", "value": "sf", "order": 2 },
                                      { "label": "Acre", "value": "acre", "order": 3 }
                                  ] 
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "leaseTerm": {
                              "show": true,
                              "properties": {
                                  "name": "leaseTerm",
                                  "label": "Lease Term",
                                  "prompt": "Select Lease Term...",
                                  "options": [
                                      { "label": "Monthly", "value": "monthly", "order": 1 },
                                      { "label": "Quarterly", "value": "quarterly", "order": 2 },
                                      { "label": "Yearly", "value": "yearly", "order": 3 }
                                  ]
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minSpace": {
                              "show": true,
                              "properties": {
                                  "name": "minSpace",
                                  "label": "Minimum Space",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              }
                          },
                          "maxSpace": {
                              "show": true,
                              "properties": {
                                  "name": "maxSpace",
                                  "label": "Total Space Available",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minPrice": {
                              "show": true,
                              "properties": {
                                  "name": "minPrice",
                                  "label": "Minimum Price",
                                  "numericOnly": true,
                                  "acceptDecimals": true,
                                  "serverDataType": "DECIMAL_CURRENCY"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "maxPrice": {
                              "show": true,
                              "properties": {
                                  "name": "maxPrice",
                                  "label": "Maximum Price",
                                  "numericOnly": true,
                                  "acceptDecimals": true,
                                  "serverDataType": "DECIMAL_CURRENCY"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "contactBrokerForPrice": {
                              "show": true,
                              "properties": {
                                  "name": "contactBrokerForPrice",
                                  "label": "Contact Broker For Pricing"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          }
                      }
                    }
                  }
                }
              }
            },
            entry: {
              spaces: []
            },
            router: {
              location: {
                pathname: '/'
              }
            }
          };
          store = mockStore(normalLeaseStore);
        });

        it("Retail:Lease - lease type should be shown", () => {
            wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
            const minPrice = wrapper.find('[name="leaseType"]');
            expect(minPrice.exists('[name="leaseType"]')).toBeTruthy();
        });

        it("Retail:Lease - measure should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const measure = wrapper.find('[name="measure"]');
          expect(measure.exists('[name="measure"]')).toBeTruthy();
        });

        it("Retail:Lease - lease term should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const leaseTerm = wrapper.find('[name="leaseTerm"]');
          expect(leaseTerm.exists('[name="leaseTerm"]')).toBeTruthy();
        });

        it("Retail:Lease - min space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minSpace = wrapper.find('[name="minSpace"]');
          expect(minSpace.exists('[name="minSpace"]')).toBeTruthy();
        });

        it("Retail:Lease - max space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const maxSpace = wrapper.find('[name="maxSpace"]');
          expect(maxSpace.exists('[name="maxSpace"]')).toBeTruthy();
        });

        it("Retail:Lease - min price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[name="minPrice"]')).toBeTruthy();
        });

        it("Retail:Lease - min price label should be Minimum Price", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[label="Minimum Price"]')).toBeTruthy();
        });

        it("Retail:Lease - max price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="maxPrice"]');
          expect(minPrice.exists('[name="maxPrice"]')).toBeTruthy();
        });

        it("Retail:Lease - contact broker for price sshould be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="contactBrokerForPrice"]');
          expect(minPrice.exists('[name="contactBrokerForPrice"]')).toBeTruthy();
        });
    });

    describe('Flex Fields - Listing Type: Sale Tests', () => {

        beforeEach(() => {
          props.listing.propertyType = 'flex';
          props.listing.listingType = 'sale';
          const flexSaleStore = {
            system: {
              configDetails: {
                loaded: true,
                config: {
                  specifications: {
                    flexFields: {
                      "values": ["flex"],
                      "sale": {
                          "leaseType": {
                              "show": false
                          },
                          "measure": {
                              "show": true,
                              "properties": {
                                  "name": "measure",
                                  "label": "Measure",
                                  "prompt": "Select Measurement...",
                                  "options": [
                                      { "label": "Per Person", "value": "person", "order": 1 },
                                      { "label": "Per Desk", "value": "desk", "order": 2 },
                                      { "label": "Per Room", "value": "room", "order": 3 }
                                  ] 
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          },
                          "leaseTerm": {
                              "show": false
                          },
                          "minSpace": {
                              "show": true,
                              "properties": {
                                  "name": "minSpace",
                                  "label": "Minimum",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              }
                          },
                          "maxSpace": {
                              "show": true,
                              "properties": {
                                  "name": "maxSpace",
                                  "label": "Maximum",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minPrice": {
                              "show": true,
                              "properties": {
                                  "name": "minPrice",
                                  "label": "Price",
                                  "numericOnly": true,
                                  "acceptDecimals": true,
                                  "serverDataType": "DECIMAL_CURRENCY"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          },
                          "maxPrice": {
                              "show": false
                          },
                          "contactBrokerForPrice": {
                              "show": true,
                              "properties": {
                                  "name": "contactBrokerForPrice",
                                  "label": "Contact Broker For Pricing"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          }
                      }
                    }
                  }
                } 
              }
            },
            entry: {
              spaces: []
            },
            router: {
              location: {
                pathname: '/'
              }
            }
          };
          store = mockStore(flexSaleStore);
        });

        it("Flex:Sale - lease type should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="leaseType"]');
          expect(minPrice.exists('[name="leaseType"]')).toBeFalsy();
        });

        it("Flex:Sale - measure should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const measure = wrapper.find('[name="measure"]');
          expect(measure.exists('[name="measure"]')).toBeTruthy();
        });

        it("Flex:Sale - measure label should be Measure", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const measure = wrapper.find('[name="measure"]');
          expect(measure.exists('[label="Measure"]')).toBeTruthy();
        });

        it("Flex:Sale - lease term should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const leaseTerm = wrapper.find('[name="leaseTerm"]');
          expect(leaseTerm.exists('[name="leaseTerm"]')).toBeFalsy();
        });

        it("Flex:Sale - min space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minSpace = wrapper.find('[name="minSpace"]');
          expect(minSpace.exists('[name="minSpace"]')).toBeTruthy();
        });

        it("Flex:Sale - max space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const maxSpace = wrapper.find('[name="maxSpace"]');
          expect(maxSpace.exists('[name="maxSpace"]')).toBeTruthy();
        });

        it("Flex:Sale - min price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[name="minPrice"]')).toBeTruthy();
        });

        it("Flex:Sale - min price label should be Price", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[label="Price"]')).toBeTruthy();
        });

        it("Flex:Sale - max price should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="maxPrice"]');
          expect(minPrice.exists('[name="maxPrice"]')).toBeFalsy();
        });

        it("Flex:Sale - contact broker for price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="contactBrokerForPrice"]');
          expect(minPrice.exists('[name="contactBrokerForPrice"]')).toBeTruthy();
        });
    });

    describe('Flex Fields - Listing Type: Lease and Sale/Lease Tests', () => {

        beforeEach(() => {
          props.listing.propertyType = 'flex';
          props.listing.listingType = 'lease';
          const flexLeaseStore = {
            system: {
              configDetails: {
                loaded: true,
                config: {
                  specifications: {
                    normalFields: {
                      "values": ["flex"],
                      "lease": {
                          "leaseType": {
                              "show": true,
                              "properties": {
                                  "name": "leaseType",
                                  "label": "Lease Type",
                                  "prompt": "Select Lease Type...",
                                  "options": [
                                      { "label": "Direct", "value": "Direct", "order": 1 },
                                      { "label": "Indirect", "value": "Indirect", "order": 2 }
                                  ]
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "measure": {
                              "show": true,
                              "properties": {
                                  "name": "measure",
                                  "label": "Measure",
                                  "prompt": "Select Measurement...",
                                  "options": [
                                      { "label": "Per Person", "value": "person", "order": 1 },
                                      { "label": "Per Desk", "value": "desk", "order": 2 },
                                      { "label": "Per Room", "value": "room", "order": 3 }
                                  ] 
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "leaseTerm": {
                              "show": true,
                              "properties": {
                                  "name": "leaseTerm",
                                  "label": "Lease Term",
                                  "prompt": "Select Lease Term...",
                                  "options": [
                                      { "label": "Monthly", "value": "monthly", "order": 1 },
                                      { "label": "Quarterly", "value": "quarterly", "order": 2 },
                                      { "label": "Yearly", "value": "yearly", "order": 3 }
                                  ]
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minSpace": {
                              "show": true,
                              "properties": {
                                  "name": "minSpace",
                                  "label": "Minimum",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              }
                          },
                          "maxSpace": {
                              "show": true,
                              "properties": {
                                  "name": "maxSpace",
                                  "label": "Maximum",
                                  "numericOnly": true,
                                  "acceptDecimals": false,
                                  "serverDataType": "INTEGER"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "minPrice": {
                              "show": true,
                              "properties": {
                                  "name": "minPrice",
                                  "label": "Price",
                                  "numericOnly": true,
                                  "acceptDecimals": true,
                                  "serverDataType": "DECIMAL_CURRENCY"
                              },
                              "grid": {
                                  "colSize": 6
                              }
                          },
                          "maxPrice": {
                              "show": false
                          },
                          "contactBrokerForPrice": {
                              "show": true,
                              "properties": {
                                  "name": "contactBrokerForPrice",
                                  "label": "Contact Broker For Pricing"
                              },
                              "grid": {
                                  "colSize": 12
                              }
                          }
                      }
                    }
                  }
                }
              }
            },
            entry: {
              spaces: []
            },
            router: {
              location: {
                pathname: '/'
              }
            }
          };
          store = mockStore(flexLeaseStore);
        });

        it("Flex:Lease - lease type should be shown", () => {
            wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
            const minPrice = wrapper.find('[name="leaseType"]');
            expect(minPrice.exists('[name="leaseType"]')).toBeTruthy();
        });

        it("Flex:Lease - measure should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const measure = wrapper.find('[name="measure"]');
          expect(measure.exists('[name="measure"]')).toBeTruthy();
        });

        it("Flex:Lease - lease term should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const leaseTerm = wrapper.find('[name="leaseTerm"]');
          expect(leaseTerm.exists('[name="leaseTerm"]')).toBeTruthy();
        });

        it("Flex:Lease - min space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minSpace = wrapper.find('[name="minSpace"]');
          expect(minSpace.exists('[name="minSpace"]')).toBeTruthy();
        });

        it("Flex:Lease - max space should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const maxSpace = wrapper.find('[name="maxSpace"]');
          expect(maxSpace.exists('[name="maxSpace"]')).toBeTruthy();
        });

        it("Flex:Lease - min price should be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[name="minPrice"]')).toBeTruthy();
        });

        it("Flex:Lease - min price label should be Price", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="minPrice"]');
          expect(minPrice.exists('[label="Price"]')).toBeTruthy();
        });

        it("Flex:Lease - max price should not be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="maxPrice"]');
          expect(minPrice.exists('[name="maxPrice"]')).toBeFalsy();
        });

        it("Flex:Lease - contact broker for price sshould be shown", () => {
          wrapper = mount(<Provider store={store}><SpecificationsView {...props} /></Provider>);
          const minPrice = wrapper.find('[name="contactBrokerForPrice"]');
          expect(minPrice.exists('[name="contactBrokerForPrice"]')).toBeTruthy();
        });
    });
  });
});