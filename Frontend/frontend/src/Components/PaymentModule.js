import React, { Component } from "react";
import {paymentTypeEnumList, frequencyEnumList} from "./Payments.js";
import "./Payments.css";

/*
    This function JSX element takes in the input:
    - Payment: The list obtained from the backend(GET api/Payments/User)
    - UserPayment: List of usernames as contributors.
*/
export default class PaymentModule extends Component {

    render () {
        const onTableClick = this.props.onTableClick

        // Map enums stored in backend to list.

        const payment = this.props.payment
        const paymentType = paymentTypeEnumList[this.props.payment["paymentType"]];
        const amount = this.props.payment["amount"];
        const startDate = this.props.payment.startDate
            .slice(0, 10)
            .split("-")
            .join("/");
        const endDate = this.props.payment.endDate
            .slice(0, 10)
            .split("-")
            .join("/");
        const frequency = frequencyEnumList[this.props.payment["frequency"]];

        //Ensure that Payments Page is rendered even when the list does not exist.
        const contributors = !this.props.contributors
            ? ["Loading..."]
            : this.props.contributors;
        const contributorsToString = contributors.join(", ");

        return (
            <div className="PaymentModule">
                <table className="PaymentModule" onClick={() => onTableClick(payment, contributors)}>
                    <tr>
                        <td className="PaymentModuleDataLeft">
                            <h6 className="PaymentModuleHeader">
                                <b>{paymentType}</b>
                            </h6>
                        </td>
                        <td className="PaymentModuleDataRight">
                            <h6 className="PaymentModuleHeader">
                                <b>
                                    {startDate} - {endDate}
                                </b>
                            </h6>
                        </td>
                    </tr>
                    <tr>
                        <td className="PaymentModuleDataLeft">
                            <h6 className="PaymentModuleData">Amount: ${amount}</h6>
                        </td>
                        <td className="PaymentModuleDataRight">
                            <h6 className="PaymentModuleData">Frequency: {frequency}</h6>
                        </td>
                    </tr>
                    <tr>
                        <td colSpan="2" className="PaymentModuleDataLeft">
                            <h6 className="PaymentModuleData">
                                Contributors: {contributorsToString}
                            </h6>
                        </td>
                    </tr>
                </table>
            </div>
        );
    }
}
