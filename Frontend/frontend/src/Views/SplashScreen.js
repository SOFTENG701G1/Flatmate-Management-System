import React from 'react';
import { Redirect } from 'react-router-dom';
import './Login.css';
import User from '../Util/User';

import Logo from '../images/logo-dark.png';

export default class SplashScreen extends React.Component {
  state = {
    redirectTo: null,
  }

  start = () => {
    this.setState({ redirectTo: '/home' });
  }

  render() {
    return (
      <div className="login-backdrop" onClick={this.start}>
        <script>
        window.onLoad = function () {
          setTimeout(this.start, 2500)
        }
      </script>
        { User.getUserState() && <Redirect to="/app/"/> }
        { this.state.redirectTo && <Redirect to={this.state.redirectTo}/> }
        <img src={Logo} alt="logo" className="splash-logo-image"/>
      </div>
    );
  }
}