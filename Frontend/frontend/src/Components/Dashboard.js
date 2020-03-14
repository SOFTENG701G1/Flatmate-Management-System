import React, {Component} from 'react';
import '../App.css';
import ChoreListComponent from './ChoreListComponent';

export default class Dashboard extends Component {
    render () {
        return (
            <ChoreListComponent/>
        );
    }
}