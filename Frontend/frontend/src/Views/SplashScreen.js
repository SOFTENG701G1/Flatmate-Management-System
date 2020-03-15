import React from 'react';
import { Redirect } from 'react-router-dom';
import './Login.css';

import Logo from '../images/logo-dark.png';

export default class SplashScreen extends React.Component {

  render() {
    return (
      <div className="login-backdrop">
        <img src={Logo} className="splash-logo-image"/>
      </div>
    );
  }
}