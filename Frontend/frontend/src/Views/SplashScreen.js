import React from 'react';
import { Redirect } from 'react-router-dom';

export default class SplashScreen extends React.Component {
    render () {
        return <div>
            Hi
            <Redirect to='/login/' />
        </div>
    }
}