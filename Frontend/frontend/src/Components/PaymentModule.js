import React from "react";
import "./PaymentModule.css";

/*
    This class exports the rendered module block of the payment data.
    This class has to be imported before being used.

    Currently this class assumes the props (short for properties) payment is of the form:
    PaymentData = {
        PaymentType: String,
        Amount: Decimal,
        StartDate: Simple DD/MM DateFormat,
        EndDate: Simple DD/MM DateFormat,
        Frequency: String,
        Contributors: String List
    }

    The class can be passed on render as <PaymentModule Payment={PaymentData} />
*/
export default class PaymentModule extends React.Component{
    render() {
        const PaymentType = this.props.Payment.PaymentType;
        const Amount = this.props.Payment.Amount;
        const StartDate = this.props.Payment.StartDate;
        const EndDate = this.props.Payment.EndDate;
        const Frequency = this.props.Payment.Frequency;
        const Contributors = this.props.Payment.Contributors;
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
}
