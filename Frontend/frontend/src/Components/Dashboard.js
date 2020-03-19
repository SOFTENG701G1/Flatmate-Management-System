import React, { Component } from 'react';
import '../App.css';
import CheckBoxListComponent from './CheckBoxListComponent';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import Col from 'react-bootstrap/col';
import DashboardPayments from './DashboardPayments';

export default class Dashboard extends Component {
    render() {
        return (
            <>
                <div>
                    <DashboardPayments />
                </div>
                <div>
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
            </>
        );
    }
}