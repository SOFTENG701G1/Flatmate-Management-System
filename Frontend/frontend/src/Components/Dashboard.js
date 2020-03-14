import React, {Component} from 'react';
import '../App.css';
import CheckBoxListComponent from './CheckBoxListComponent';

export default class Dashboard extends Component {
    render () {
        return (
            <CheckBoxListComponent 
                title="Chores"
                listItems={["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"]}
            />
        );
    }
}