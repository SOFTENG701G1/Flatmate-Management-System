import React, { Component } from 'react';
import APIRequest from "../Util/APIRequest";
import PaymentModule from "./PaymentModule";


export default class DashboardPayments extends Component {
    constructor() {
        super();

        this.state = {
            payments: [],
            contributionPayments: []
        };
    }

    async componentDidMount() {
        // Obtain payments associated with the logged in user.
        await APIRequest.obtainUserPayments().then(data => {
            data = data.sort((a, b) => a.startDate.localeCompare(b.startDate)).splice(0, Math.min(4, data.length));
            this.setState({
                payments: data
            });
        });

        for (let i = 0; i < this.state.payments.length; i++) {
            await APIRequest.obtainPaymentContributors(
                this.state.payments[i]["id"]
            ).then(usersJSON => {
                const listUser = [];
                for (let j = 0; j < usersJSON.length; j++) {
                    listUser.push(usersJSON[j]["userName"]);
                }
                this.addUserPayment(listUser);
            });
        }
    }

    addUserPayment = users => {
        let contributionPayments = this.state.contributionPayments;
        contributionPayments.push(users);
        this.setState({
            contributionPayments: contributionPayments
        });
    };

    render () {
        const paymentComponenets = [];
        const payments = this.state.payments;
        const contributorsPayments = this.state.contributionPayments;
        for (let i = 0; i < this.state.payments.length; i++) {
            const paymentData = payments[i];
            const contributors = contributorsPayments[i];
            paymentComponenets.push(
                <PaymentModule
                    payment={paymentData}
                    contributors={contributors}
                    onTableClick={() => { }}
                />
            );
        }

        return (
            <div>
                <p><b>Upcoming Payments</b></p>
                {paymentComponenets}
            </div>
        );
    }
}