﻿import { runWithAdal } from 'react-adal';
import { authContext } from './adalConfig';
 
const DO_NOT_LOGIN = false;

runWithAdal(authContext, () => {
 
  // eslint-disable-next-line
  require('./indexApp.tsx');
 
},DO_NOT_LOGIN);