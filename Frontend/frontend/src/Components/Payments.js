import React, { Component } from "react";
import NewPayment from "./NewPayment"
import ViewPayment from "./ViewPayment"
import "./Payments.css"
import APIRequest from "../Util/APIRequest";

/*
    This class renders the Payments page.
*/
export default class Payments extends Component {
    constructor() {
        super();
        this.state = {
            showView: false,
            show: false,
            // Currently are dummy data
            Payments: [],
            ContributionPayments: [],
            currentPayment: {  
                PaymentType: "",
                Amount: 0,
                StartDate: "",
                EndDate: "",
                Frequency: "",
                Contributors: ["", ""]
            }
        }
    }

    async componentDidMount(){
        // Obtain payments associated with the logged in user.
        await APIRequest.obtainUserPayments().then(
            data => {
                this.setState({
                    Payments: data
                })
            }
        )
            
        for(let i = 0; i < this.state.Payments.length; i++){
            await APIRequest.obtainPaymentContributors(this.state.Payments[i]["id"]).then(
                UsersJSON => {
                    const list_user = []
                    for(let j = 0; j < UsersJSON.length; j++){
                        list_user.push(UsersJSON[j]["userName"]);
                    }
                    this.addUserPayment(list_user)
                }
            )
            
        }
    }

    addUserPayment = (Users) => {
        let ContributionPayments = this.state.ContributionPayments;
        ContributionPayments.push(Users);
        this.setState({
            ContributionPayments: ContributionPayments
        })
    }

    //Methods for opening new payment component
    _handleOpen = () => {
        this.setState({
            show: true
        })
   }

    _handleClose = () => {
        this.setState({
            show: false
        })
    }

    //Methods for opening view payment component
    _handleOpenView = () => {
        this.setState({
            showView: true
        })
    }

    _handleCloseView = () => {
        this.setState({
            showView: false
        })
    }

    _handleEdit = () => {
        this.setState({
            show: true,
            showView: false
        })
    }

    _handleTableClick = (payment) => {
        this.setState({
            currentPayment: payment
        }, 
        () => this._handleOpenView())
    }

    render() {
        const FixedPaymentsHtml = [];
        const VariablePaymentsHtml = [];
        const Payments = this.state.Payments;
        const ContributorsPayments = this.state.ContributionPayments;
        for(let i = 0; i < this.state.Payments.length; i++){
            const PaymentData = Payments[i];
            const Contributors = ContributorsPayments[i];
            if(PaymentData["fixed"]) FixedPaymentsHtml.push(
                <PaymentModule 
                    Payment={PaymentData} 
                    Contributors={Contributors}
                    onTableClick={this._handleTableClick}
                />
            )
            else VariablePaymentsHtml.push(
                <PaymentModule 
                    Payment={PaymentData}  
                    Contributors={Contributors}
                    onTableClick={this._handleTableClick}
                />
            )
            
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
                                <button className="NewPaymentButton">
                                    Add new
                                </button>
                            </span>
                            <NewPayment 
                                onClose={this._handleClose} 
                                show={this.state.show} 
                            />
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
                        <ViewPayment 
                            onCloseView={this._handleCloseView} 
                            showView={this.state.showView} 
                            onEdit={this._handleEdit} 
                            payment={this.state.currentPayment}
                        />
                    </tr>
                </table>
            </div>
        )
    }
}


/*
    This function JSX element takes in the input:
    - Payment: The list obtained from the backend(GET api/Payments/User)
    - UserPayment: List of usernames as contributors.
*/
function PaymentModule(props) {
    // Map enums stored in backend to list.
    const PaymentTypeEnumList = ["Rent", "Electricity", "Water", "Internet", "Groceries", "Other"];
    const FrequencyEnumList = ["OneOff", "Weekly", "Fortnightly", "Monthly"];

    const PaymentType = PaymentTypeEnumList[props.Payment["paymentType"]];
    const Amount = props.Payment["amount"];
    const StartDate = props.Payment.startDate.slice(0, 10).split("-").join("/");
    const EndDate = props.Payment.endDate.slice(0, 10).split("-").join("/");
    const Frequency = FrequencyEnumList[props.Payment["frequency"]];

    //Ensure that Payments Page is rendered even when the list does not exist.
    const Contributors = !props.Contributors ? ["Loading..."]: props.Contributors; 
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
