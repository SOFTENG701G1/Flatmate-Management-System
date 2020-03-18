import React, { Component } from "react";
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Cross from '../images/cross.png';
import Delete from '../images/delete.png';
import Edit from '../images/edit.png';
import "./ViewPayment.css";

/*
    This class renders the View Payments page.
    Dummy data is currently used
*/

export default class ViewPayment extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        const { showView, onCloseView, onEdit, payment } = this.props
        return (
            <>
                <Modal show={showView} onHide={onCloseView} backdrop={true}>
                    <Modal.Header>
                        <span className="CrossButton">
                            <Button className="CrossButton" variant="secondary" onClick={onCloseView}>
                                <img src={Cross} alt="cross" className="cross-image" />
                            </Button>
                        </span>
                        <Modal.Title>
                            <h1 class="modal-title w-100 text-center"> {payment.PaymentType}</h1>
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <table className="InputTable">
                            <tr className="RowOne">
                                <td className="RowOneColOne" colSpan="2">
                                    <form>
                                        <label className="AmountLabel"> Amount: </label>
                                        <label className="Amount"> {payment.Amount} </label>
                                    </form>
                                </td>
                                <td className="RowOneColTwo">
                                    <form>
                                        <label className="StartDateLabel"> Start Date: </label>
                                        <label className="StartDate"> {payment.StartDate} </label>
                                    </form>
                                </td>
                            </tr>
                            <tr className="RowTwo">
                                <td colSpan="2">
                                    <form>
                                        <label className="PaidToLabel"> Paid To: </label>
                                        <label className="PaidTo"> Ken </label>
                                    </form>
                                </td>
                                <td>
                                    <form>
                                        <label className="EndDateLabel"> End Date: </label>
                                        <label className="EndDate"> {payment.EndDate} </label>
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="2">
                                    <form>
                                        <label className="AccountLabel"> Account: </label>
                                        <label className="Account"> 0800 83 83 83 </label>
                                    </form>
                                </td>
                                <td>
                                    <form>
                                        <label className="FrequencyLabel"> Frequency: </label>
                                        <label className="Frequency"> {payment.Frequency} </label>
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="ContributorsPendingLabel"> Contributors Pending: </label>
                                        <label className="ContributorsPending"> </label>
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="ContributorsPaidLabel"> Contributors Paid: </label>
                                        <label className="ContributorsPaid"> {payment.Contributors.join(", ")} </label>
                                    </form>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="3">
                                    <form>
                                        <label className="DescriptionLabel"> Description: </label>
                                        <label className="Description"> Pikachu, I choose you! </label>
                                    </form>
                                </td>
                            </tr>
                        </table>
                    </Modal.Body>
                    <Modal.Footer>
                        <span className="DeleteButton">
                            <Button className="DeleteButton" variant="secondary" onClick={onCloseView}>
                                <img src={Delete} alt="delete" className="delete-image" />
                            </Button>
                        </span>
                        <span className="EditButton">
                            <Button className="EditButton" variant="secondary" onClick={onEdit}>
                                <img src={Edit} alt="edit" className="edit-image" />
                            </Button>
                        </span>
                        <span className="PaidButton">
                            <Button className="PaidButton" variant="primary" onClick={onCloseView}>
                                Paid
                            </Button>
                        </span>
                    </Modal.Footer>
                </Modal>
            </>
        );
    }
}
