import React, { Component } from "react";
import NewPayment from "./NewPayment"
import "./Payments.css"

/*
    This class renders the Payments page.
*/
export default class Payments extends Component {
    constructor() {
        super();
        this.state = {
            show: false,
            // Currently are dummy data
            FixedPayments: [{
                PaymentType: "Rent",
                Amount: 240,
                StartDate: "15/03",
                EndDate: "15/12",
                Frequency: "Monthly",
                Contributors: ["Misty", "Brock"]
            }, {
                PaymentType: "WiFi",
                Amount: 100,
                StartDate: "15/03",
                EndDate: "15/12",
                Frequency: "Monthly",
                Contributors: ["Misty", "Brock", "Samuel"]
            }],
            VariablePayments: [{
                PaymentType: "Water",
                Amount: 140,
                StartDate: "15/03",
                EndDate: "15/12",
                Frequency: "Monthly",
                Contributors: ["Misty", "Brock"]
            }, {
                PaymentType: "Electricity",
                Amount: 100,
                StartDate: "15/03",
                EndDate: "15/12",
                Frequency: "Monthly",
                Contributors: ["Misty", "Brock", "Samuel"]
            }]
        }
    }

    handleOpen = () => {
        this.setState({
            show: true
        })
    }

    handleClose = () => {
        this.setState({
            show: false
        })
    }

    render() {
        const FixedPaymentsHtml = [];
        const VariablePaymentsHtml = []
        this.state.FixedPayments.forEach(
            PaymentData => {
                FixedPaymentsHtml.push(
                    <PaymentModule Payment={PaymentData} />
                )
            }
        )

        this.state.VariablePayments.forEach(
            PaymentData => {
                VariablePaymentsHtml.push(
                    <PaymentModule Payment={PaymentData} />
                )
            }
        )

        return (
            <div className="PaymentPage">
                <table className="PaymentTable">
                    <tr>
                        <td colSpan="2" className="PaymentPageTitle">
                            <span className="PaymentPageTitle">
                                <h2 className="PaymentsTitle">Payments</h2>
                            </span>
                            <span className="NewPaymentButton" onClick={this.handleOpen}>
                                <button className="NewPaymentButton">
                                    Add new
                                </button>
                            </span>
                            <NewPayment onClose={this.handleClose} show={this.state.show} />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4 className="Subtitle">
                                Fixed
                            </h4>
                        </td>
                        <td>
                            <h4 className="Subtitle">
                                Variable
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            {FixedPaymentsHtml}
                        </td>
                        <td>
                            {VariablePaymentsHtml}
                        </td>
                    </tr>
                </table>
            </div>
        )
    }
}

/*
    This function JSX element takes in the JSON list which is of the form:
    {
        PaymentType: String,
        Amount: Decimal(10,2),
        StartDate: DateTime String,
        EndDate: DateTime String,
        Frequency: String,
        Contributors: String[]
    }
*/
function PaymentModule(props) {
    const PaymentType = props.Payment.PaymentType;
    const Amount = props.Payment.Amount;
    const StartDate = props.Payment.StartDate;
    const EndDate = props.Payment.EndDate;
    const Frequency = props.Payment.Frequency;
    const Contributors = props.Payment.Contributors;
    const ContributorsToString = Contributors.join(", ");
    return (
        <div className="PaymentModule">
            <table className="PaymentModule">
                <tr>
                    <td className="PaymentModuleDataLeft">
                        <h6 className="PaymentModuleHeader">
                            <b>{PaymentType}</b>
                        </h6>
                    </td>
                    <td className="PaymentModuleDataRight">
                        <h6 className="PaymentModuleHeader">
                            <b>{StartDate}-{EndDate}</b>
                        </h6>
                    </td>
                </tr>
                <tr>
                    <td className="PaymentModuleDataLeft">
                        <h6 className="PaymentModuleData">
                            Amount: ${Amount}
                        </h6>
                    </td>
                    <td className="PaymentModuleDataRight">
                        <h6 className="PaymentModuleData">
                            Frequency: {Frequency}
                        </h6>
                    </td>
                </tr>
                <tr>
                    <td colSpan="2" className="PaymentModuleDataLeft">
                        <h6 className="PaymentModuleData">
                            Contributors: {ContributorsToString}
                        </h6>
                    </td>
                </tr>
            </table>
        </div>
    )
}