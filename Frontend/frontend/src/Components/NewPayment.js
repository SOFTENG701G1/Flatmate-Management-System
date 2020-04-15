import React, { Component } from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import Cross from "../images/cross.png";
import "./NewPayment.css";
import APIRequest from "../Util/APIRequest";

/*
    This class renders the New Payments page.
*/

export default class NewPayment extends Component {
  constructor(props) {
    super(props);

    this.state = {
      amount: undefined,
      startDate: undefined,
      paidTo: undefined,
      endDate: undefined,
      account: undefined,
      frequency: undefined,
      contributorsPending: undefined,
      contributorsPaid: undefined,
      description: undefined,
      listOfIds: undefined,
    };

    this.createNewPayment = this.createNewPayment.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async createNewPayment(event) {
    event.preventDefault();
    this.contributorsPending = {
      BeboBryan: "0",
      YinWang: "0",
      TreesAreGreen: "0",
    };
    await APIRequest.getUserIdsByUsername(this.contributorsPending).then(
      (data) => {
        if (!data) {
          console.log("No response");
        } else {
          this.state.listOfIds = Object.keys(data.userID).map(function (key) {
            return data.userID[key];
          });
        }
      }
    );
    let paymentResult = await APIRequest.createNewPayment(
      parseInt(this.state.amount),
      this.state.startDate,
      this.state.paidTo,
      this.state.endDate,
      this.state.account,
      parseInt(this.state.frequency),
      this.state.contributorsPending,
      this.state.contributorsPaid,
      this.state.description,
      this.state.listOfIds
    );
    if (paymentResult.ok) {
      this.forceUpdate(); // Triggers re-render, which will activate redirect now user is setup
      this.props.onClose();
    } else {
      switch (paymentResult.status) {
        case 401:
          this.setState({ error: "Not authorized user" });
          break;
        default:
          this.setState({ error: "Unknown error (check your internet)." });
          break;
      }
    }
  }

  bindInput(event) {
    let target = event.target;
    this.setState({
      [target.name]: target.value,
    });
  }

  // Toggle modal's status to open/close
  handleToggle() {
    this.setState({
      isModalOpen: !this.state.isModalOpen,
    });
  }

  render() {
    const { show, onClose } = this.props;

    return (
      <>
        <Modal show={show} onHide={onClose} backdrop={true}>
          <Modal.Header>
            <span className="CrossButton">
              <Button
                className="CrossButton"
                variant="secondary"
                onClick={onClose}
              >
                <img src={Cross} alt="cross" className="cross-image" />
              </Button>
            </span>
            <Modal.Title>
              <h1 class="modal-title w-100 text-center"> Rent</h1>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <table className="InputTable">
              <tr className="RowOne">
                <td className="RowOneColOne" colSpan="2">
                  <form>
                    <label className="AmountLabel"> Amount: </label>
                    <input
                      className="AmountInput"
                      type="text"
                      name="amount"
                      onChange={this.bindInput}
                      placeholder="Amount*"
                      defaultValue={this.state.amount}
                    />
                  </form>
                </td>
                <td className="RowOneColTwo">
                  <form>
                    <label className="StartDateLabel"> Start Date: </label>
                    <input
                      className="StartDateInput"
                      type="text"
                      name="startDate"
                      onChange={this.bindInput}
                      placeholder="Start Date*"
                      defaultValue={this.state.startDate}
                    />
                  </form>
                </td>
              </tr>
              <tr className="RowTwo">
                <td colSpan="2">
                  <form>
                    <label className="PaidToLabel"> Paid To: </label>
                    <input
                      className="PaidToInput"
                      type="text"
                      name="paidTo"
                      onChange={this.bindInput}
                      placeholder="Paid To*"
                      defaultValue={this.state.paidTo}
                    />
                  </form>
                </td>
                <td>
                  <form>
                    <label className="EndDateLabel"> End Date: </label>
                    <input
                      className="EndDateInput"
                      type="text"
                      name="endDate"
                      onChange={this.bindInput}
                      placeholder="End Date*"
                      defaultValue={this.state.endDate}
                    />
                  </form>
                </td>
              </tr>
              <tr>
                <td colSpan="2">
                  <form>
                    <label className="AccountLabel"> Account: </label>
                    <input
                      className="AccountInput"
                      type="text"
                      name="account"
                      onChange={this.bindInput}
                      placeholder="Account*"
                      defaultValue={this.state.account}
                    />
                  </form>
                </td>
                <td>
                  <form>
                    <label className="FrequencyLabel"> Frequency: </label>
                    <input
                      className="FrequencyInput"
                      type="text"
                      name="frequency"
                      onChange={this.bindInput}
                      placeholder="Frequency*"
                      defaultValue={this.state.frequency}
                    />
                  </form>
                </td>
              </tr>
              <tr>
                <td colSpan="3">
                  <form>
                    <label className="ContributorsPendingLabel">
                      {" "}
                      Contributors Pending:{" "}
                    </label>
                    <input
                      className="ContributorsPendingInput"
                      type="text"
                      contributorsPending="contributorsPending"
                      onChange={this.bindInput}
                      placeholder="Contributors Pending*"
                      defaultValue={this.state.contributorsPending}
                    />
                  </form>
                </td>
              </tr>
              <tr>
                <td colSpan="3">
                  <form>
                    <label className="ContributorsPaidLabel">
                      {" "}
                      Contributors Paid:{" "}
                    </label>
                    <input
                      className="ContributorsPaidInput"
                      type="text"
                      contributorPaids="contributorsPaid"
                      onChange={this.bindInput}
                      placeholder="Contributors Paid*"
                      defaultValue={this.state.contributorsPaid}
                    />
                  </form>
                </td>
              </tr>
              <tr>
                <td colSpan="3">
                  <form>
                    <label className="DescriptionLabel"> Description: </label>
                    <input
                      className="DescriptionInput"
                      type="text"
                      name="description"
                      onChange={this.bindInput}
                      placeholder="Description*"
                      defaultValue={this.state.description}
                    />
                  </form>
                </td>
              </tr>
            </table>
          </Modal.Body>
          <Modal.Footer>
            <span className="CancelButton">
              <Button
                className="CancelButton"
                variant="secondary"
                onClick={onClose}
              >
                Cancel
              </Button>
            </span>
            <span className="SaveButton">
              <Button
                className="SaveButton"
                variant="primary"
                onClick={this.createNewPayment}
              >
                Save
              </Button>
            </span>
          </Modal.Footer>
        </Modal>
      </>
    );
  }
}
