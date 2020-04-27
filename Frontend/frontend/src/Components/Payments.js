import React, { Component } from "react";
import PaymentModule from "./PaymentModule";
import NewPayment from "./NewPayment";
import ViewPayment from "./ViewPayment";
import "./Payments.css";
import APIRequest from "../Util/APIRequest";

/*
    This class renders the Payments page.
*/
export default class Payments extends Component {
    constructor() {
        super();

        this.state = {
            flatMembers: [""],
            showView: false,
            show: false,
            payments: [],
            contributionPayments: [],
            currentPayment: {
                id: 0,
                paymentType: 0,
                amount: 0,
                startDate: "",
                endDate: "",
                frequency: ""
            },
            currentContributors: [""]
        };
    }

    async getAllMembers() {
        const memberResult = await APIRequest.getFlatMembers();
        return memberResult.json();
      }

    async componentDidMount() {
        // Obtain payments associated with the logged in user.
        await APIRequest.obtainUserPayments().then(data => {
            if(!data) data = [];
            this.setState({
                payments: data
            });
        });

        for (let i = 0; i < this.state.payments.length; i++) {
            await APIRequest.obtainPaymentContributors(
                this.state.payments[i]["id"]
            ).then(usersJSON => {
                if(!usersJSON) usersJSON = [];
                const listUser = [];
                for (let j = 0; j < usersJSON.length; j++) {
                    listUser.push(usersJSON[j]["userName"]);
                }
                this.addUserPayment(listUser);
            });
        }

        this.getAllMembers().then((members) => {
            console.log(members);
            console.log(members.flatMembers);

            const flatMembers =[];
            let userName = '';
            for (let i=0; i< members.flatMembers.length; i++){
                userName = members.flatMembers[i]["userName"];
                console.log(userName);
                flatMembers.push(userName);
            }

            this.setState({
                flatMembers: flatMembers
            });
        });
    }

    addUserPayment = users => {
        let contributionPayments = this.state.contributionPayments;
        contributionPayments.push(users);
        this.setState({
            contributionPayments: contributionPayments
        });
    };

    //Methods for opening new payment component
    _handleOpen = () => {
        this.setState({
            show: true
        });
    };

    _handleClose = () => {
        this.setState({
            show: false
        });
    };

    //Methods for opening view payment component
    _handleOpenView = () => {
        this.setState({
            showView: true
        });
    };

    _handleCloseView = () => {
        this.setState({
            showView: false
        });
    };

    _handleEdit = () => {
        this.setState({
            show: true,
            showView: false
        });
    };

    _handleTableClick = (payment, contributors) => {
        this.setState(
            {
                currentPayment: payment,
                currentContributors: contributors
            },
            () => this._handleOpenView()
        );
    };

    render() {
        const fixedPaymentsHtml = [];
        const variablePaymentsHtml = [];
        const payments = this.state.payments;
        const contributorsPayments = this.state.contributionPayments;
        const members = this.state.flatMembers;
        
        for (let i = 0; i < payments.length; i++) {
            const paymentData = payments[i];
            const contributors = contributorsPayments[i];
            if (paymentData["fixed"])
                fixedPaymentsHtml.push(
                    <PaymentModule
                        payment={paymentData}
                        contributors={contributors}
                        onTableClick={this._handleTableClick}
                    />
                );
            else
                variablePaymentsHtml.push(
                    <PaymentModule
                        payment={paymentData}
                        contributors={contributors}
                        onTableClick={this._handleTableClick}
                    />
                );
        }

        return (
            <div className="PaymentPage">
                <table className="PaymentTable">
                    <tr>
                        <td colSpan="2" className="PaymentPageTitle">
                            <span className="PaymentPageTitle">
                                <h2 className="PaymentsTitle">Payments</h2>
                            </span>
                            <span className="NewPaymentButton" onClick={this._handleOpen}>
                                <button className="NewPaymentButton">Add new</button>
                            </span>
                            <NewPayment onClose={this._handleClose} show={this.state.show} flatMembers={members} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 className="Subtitle">Fixed</h4>
                        </td>
                        <td>
                            <h4 className="Subtitle">Variable</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>{fixedPaymentsHtml}</td>
                        <td>{variablePaymentsHtml}</td>
                        <ViewPayment
                            onCloseView={this._handleCloseView}
                            showView={this.state.showView}
                            onEdit={this._handleEdit}
                            payment={this.state.currentPayment}
                            contributors={this.state.currentContributors}
                        />
                    </tr>
                </table>
            </div>
        );
    }
}

export const paymentTypeEnumList = [
    "Rent",
    "Electricity",
    "Water",
    "Internet",
    "Groceries",
    "Other"
];
export const frequencyEnumList = ["OneOff", "Weekly", "Fortnightly", "Monthly"];
