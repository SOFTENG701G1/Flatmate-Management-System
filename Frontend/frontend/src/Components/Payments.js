import React from "react";
import PaymentModule from "./PaymentModule"
import "./Payments.css"

/*
    This class renders the Payments page.
*/
export default class PaymentSchedule extends React.Component {
    constructor () {
        super();

        // Currently are dummy data
        this.state = {
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

    render () {
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
                            <span className="NewPaymentButton">
                                <button className="NewPaymentButton">
                                    Add new
                                </button>
                            </span>
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