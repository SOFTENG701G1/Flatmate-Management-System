import React from 'react';
import { Redirect } from 'react-router-dom';

import './Navigation.css';
import User from '../Util/User';

export default class RequireLogin extends React.Component {
  render() {
    return <div>
      {/* Redirects to login page if there's no logged in user */}
      { !User.getUserState() ? <Redirect to="/login/"/> : ""}
    </div>
  }
}