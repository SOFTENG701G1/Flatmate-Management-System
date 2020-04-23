import React, { Component } from 'react';
import CheckBoxListComponent from './CheckBoxListComponent';
import DashboardPayments from './DashboardPayments';
import DashboardChores from './DashboardChores';
import './Dashboard.css';

export default class Dashboard extends Component {
    render () {
        return (
            <div className="DashboardContainer">
                <div className="LeftColumn">
                    <DashboardPayments />
                </div>
                <div className="RightColumn">
                    <DashboardChores />
                    
                    <hr className="LineDivider" />
                    <CheckBoxListComponent
                        title="Shopping List"
                        listItems={["Eggs", "Milk", "TP", "Cereal", "Pasta", "Sweetcorn"]}
                        onListItemChanged={() => {}}
                        isChecked={[1,0,1,0,1,0]}
                        relatedId={[1,2,3,4,5,6]}
                    />
                </div>
            </div>
        );
    }
}