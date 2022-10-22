import { createStore, compose, applyMiddleware, combineReducers } from 'redux';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';
import { identity } from 'ramda'
import { History } from 'history';
import reducers from '../redux/reducers'

const createReducers = (history: History) =>
  combineReducers({
    router: connectRouter(history),
    ...reducers
});

/* istanbul ignore next */
const allowDebugging =
  process.env.NODE_ENV !== 'production' ||
  (localStorage && localStorage.getItem('reactDebug') === 'yes');
  
const buildStore = (initialState: any, history: History) => {

  /*  eslint-disable no-underscore-dangle */
  const devToolsExt =
    allowDebugging && window.devToolsExtension
      ? window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
      : identity;

  const middleWare = false
    ? applyMiddleware(routerMiddleware(history), thunkMiddleware, createLogger())
    : applyMiddleware(routerMiddleware(history), thunkMiddleware);

  const store = createStore(
    createReducers(history),
    initialState as any,
    compose(
      middleWare,
      devToolsExt
    )
  );

  return store;
};
export default buildStore;