import React from 'react';
import User from '../Util/User';
import { Redirect } from 'react-router-dom';

export default class Logout extends React.Component {
    render () {
        User.clearUserState();
        return <Redirect to='/' />;
    }
}