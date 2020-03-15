import React from 'react';
import { Row } from 'react-bootstrap';

import Payment from './Payment';

export default class PaymentList extends React.Component {
    render() {
        //TODO #98 retrive data when componentDidMount()
        const samplePayment = {type: "Rent", start: "Now", end: "Then", amount: 230.99, frequency: "Weekly", contributors: ["Ash", "Misty"]};
        const data = [samplePayment, samplePayment, samplePayment, samplePayment];
        const listItems = data.map((d) => <Payment details={d}/>);

        return (
            <div className="PaymentList">
                <h5>Upcoming Payments</h5>
                {listItems}
            </div>
        );
    }1
}