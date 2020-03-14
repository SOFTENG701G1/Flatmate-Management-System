import React from 'react';
import { Link } from 'react-router-dom';

import './Login.css';
import APIRequest from '../Util/APIRequest';

export default class Login extends React.Component {
  constructor (props) {
    super(props);

    this.state = {
      username: undefined,
      password: undefined
    }

    this.login = this.login.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async login (event) {
    event.preventDefault();
    this.setState({ error: ""});
    if (!this.state.username || !this.state.password) {
      this.setState({ error: "Please enter a username and password. "});
      return;
    }
    
    let loginResult = await APIRequest.login(this.state.username, this.state.password);

    if (loginResult.ok) {
      console.log(await loginResult.json())
    } else {
      switch (loginResult.status) {
        case 404:
          this.setState({ error: "There's no such user. "});
          break;
        case 403:
          this.setState({ error: "Invalid password. "});
          break;
        default:
          this.setState({ error: "Unknown error (check your internet). "});
          break;
      }
    }
  }
  
  bindInput (event) {
    let target = event.target;

    this.setState({
      [target.name]: target.value
    });
  }

  render() {
    return <div className="login-container">
        <div className='login-backdrop'></div>
        <div className='login-icon'></div>
        <h2> Login to your Account </h2>
        <form action="#" method="POST">
          <input type='text' name='username' onChange={this.bindInput} placeholder='Username'/>
          <input type='password' name='password' onChange={this.bindInput} placeholder='Password'/>
          { this.state.error ? <div className='login-error'> { this.state.error } </div> : '' }
          <input type='submit' value='Login' onClick={this.login}/>
        </form>
      </div>
  }
}