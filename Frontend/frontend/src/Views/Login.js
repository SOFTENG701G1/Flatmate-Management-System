import React from 'react';
import { Link } from 'react-router-dom';

import './Login.css';

export default class Login extends React.Component {
  render() {
    return <div className="login-container">
        <div className='login-backdrop'></div>
        <div className='login-icon'></div>
        <h1> Login to your Account </h1>
        <input type='text' placeholder='Username'/>
        <input type='text' placeholder='Password'/>
      </div>
  }
}