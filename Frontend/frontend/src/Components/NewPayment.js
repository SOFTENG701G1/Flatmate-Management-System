import React, { Component } from "react";
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
// import Cross from '.../images/cross.png';
import "./NewPayment.css";



export default class NewPayment extends Component {


    constructor() {
        super();
        this.state = {
            show: false
        }


    }



    handleClick = () => {
        this.setState({ show: true });
    };



    handleShow = () => {
        this.setState({ show: true });
    }

    handleClose = () => {
        this.setState({ show: false });
    }


    render() {
        return (
            <>
                <Button variant="primary" onClick={this.handleShow}>
                    Launch demo modal
                                </Button>
                <Modal show={this.state.show} onHide={this.handleClose} backdrop={true}>
                    <Modal.Header>
                       
                        {/* image button */}
                        {/* <span className="CancelButton">
                            <Button className="CancelButton" variant="secondary" onClick={this.handleClose}>

                            <img src={Cross} alt="cross" className="cross-image"/>
                                        </Button>
                        </span> */}
                        <Modal.Title>
                            <h1  class="modal-title w-100 text-center"> Rent</h1>
                        </Modal.Title>

                    </Modal.Header>
                    <Modal.Body>
                        <table className="InputTable">
                            <tr className="RowOne">
                                <td className="RowOneColOne" colSpan="2">
                                    <form>
                                        <label className="AmountLabel"> Amount: </label>
                                        <input className="AmountInput" type="text" amount="amount" />
                                    </form>
                                </td>
                                <td className="RowOneColTwo">
                                    <form>
                                        <label className="StartDateLabel"> Start Date: </label>
                                        <input className="StartDateInput" type="text" startDate="startDate" />
                                    </form>
                                </td>
                            </tr>
                            <tr className="RowTwo">
                                <td colSpan="2">
                                    <form>
                                        <label className="PaidToLabel"> Paid To: </label>
                                        <input className="PaidToInput" type="text" paidTo="paidTo" />
                                    </form>
                                </td>
                                <td>
                                    <form>
                                        <label className="EndDateLabel"> End Date: </label>
                                        <input className="EndDateInput" type="text" endDate="endDate" />
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="2">
                                    <form>
                                        <label className="AccountLabel"> Account: </label>
                                        <input className="AccountInput" type="text" Account="Account" />
                                    </form>
                                </td>
                                <td>
                                    <form>
                                        <label className="FrequencyLabel"> Frequency: </label>
                                        <input className="FrequencyInput" type="text" frequency="frequency" />
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="ContributorsPendingLabel"> Contributors Pending: </label>
                                        <input className="ContributorsPendingInput" type="text" contributorsPending="contributorsPending" />
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="ContributorsPaidLabel"> Contributors Paid: </label>
                                        <input className="ContributorsPaidInput" type="text" contributorPaids="contributorsPaid" />
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="DescriptionLabel"> Description: </label>
                                        <input className="DescriptionInput" type="text" description="description" />
                                    </form>
                                </td>
                            </tr>
                        </table>
                    </Modal.Body>
                    <Modal.Footer>
                        <span className="CancelButton">
                            <Button className="CancelButton" variant="secondary" onClick={this.handleClose}>
                                Cancel
                                        </Button>
                        </span>
                        <span className="SaveButton">
                            <Button className="SaveButton" variant="primary" onClick={this.handleClose}>
                                Save
                                        </Button>
                        </span>
                    </Modal.Footer>
                </Modal>
            </>
        );
    }
}