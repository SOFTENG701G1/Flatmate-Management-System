import React, { Component } from 'react';
import CheckBoxListComponent from './CheckBoxListComponent';
import DashboardPayments from './DashboardPayments';
import './Dashboard.css';

export default class Dashboard extends Component {
    render () {
        return (
            <div className="DashboardContainer">
                <div className="LeftColumn">
                    <DashboardPayments />
                </div>
                <div className="RightColumn">
                    <CheckBoxListComponent
                        title="Chores"
                        listItems={["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"]}
                    />
                    <hr className="LineDivider" />
                    <CheckBoxListComponent
                        title="Shopping List"
                        listItems={["Eggs", "Milk", "TP", "Cereal", "Pasta", "Sweetcorn"]}
                    />
                </div>
            </div>
        );
    }
}