import React from 'react';
import { Redirect, Link } from 'react-router-dom';

import User from '../Util/User';

import './Login.css';
import Logo from '../images/logo-house-blue.png';

export default class Login extends React.Component {

  render() {
    return (
      <div className="login-backdrop">
        { User.getUserState() && <Redirect to="/app/"/> }
        <div className='login-container'>
          <img src={Logo} alt="logo" className="logo-image"/>
          <h2 style={{ marginBottom: 10 }}>FLATMATE</h2>
          <p style={{ margin: 0 }}>Making flat management easier</p>
          <form action="#" method="POST" style={{ marginTop: 20 }}>
            <Link to="/login"><input type='button' value='Login'/></Link>
            <Link to="/register"><input type='button' value='Sign up'/></Link>
          </form>
        </div>
      </div>
    );
  }
}