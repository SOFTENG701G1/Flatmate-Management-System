import React from 'react';
import '../App.css';
import './Payment.css';

export default class Payment extends React.Component {
    render() {
        const data = this.props.details;
        return (
            <div className="PaymentContainer">
                <div>
                    <h6>{data.type}</h6>
                    <p>Amount: ${data.amount}</p>
                    <p>Contributors: {data.contributors.join(", ")}</p>
                </div>
                <div className="PaymentContainerRight">
                    <h6>{data.start} - {data.end}</h6>
                    <p>Frequency: {data.frequency}</p>
                </div>
            </div>
        );
    }
}