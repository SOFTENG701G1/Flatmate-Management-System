import React, { Component } from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import Cross from "../images/cross.png";
import "./NewPayment.css";
import APIRequest from "../Util/APIRequest";
import UserList from "./UserList";
import {
  TextField,
  MenuItem,
  Select,
  InputLabel,
  InputBase,
} from "@material-ui/core";
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
    this.updateContributorsPaid = this.updateContributorsPaid.bind(this);
    this.updateContributorsPending = this.updateContributorsPending.bind(this);
  }

  async createNewPayment(event) {
    event.preventDefault();

    await APIRequest.getUserIdsByUsername(this.state.contributorsPending).then(
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

    console.log(this.state);
  }

  updateContributorsPaid(items) {
    debugger;
    this.setState({
      contributorsPaid: items,
    });
  }

  updateContributorsPending(items) {
    this.setState({
      contributorsPending: items,
    });
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
                  <TextField
                    id="datetime-local"
                    label="Start date"
                    type="date"
                    name="startDate"
                    size="large"
                    defaultValue="2017-05-24T10:30:00Z"
                    InputLabelProps={{
                      shrink: true,
                      className: "date",
                    }}
                    InputProps={{
                      className: "date",
                    }}
                    onChange={this.bindInput}
                  />
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
                  <TextField
                    id="datetime-local"
                    label="End date"
                    type="date"
                    name="endDate"
                    size="large"
                    defaultValue="2017-05-24T10:30:00Z"
                    InputLabelProps={{
                      shrink: true,
                      className: "date",
                    }}
                    InputProps={{
                      className: "date",
                    }}
                    onChange={this.bindInput}
                  />
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
                  <InputLabel className={"frequency"}>Frequency</InputLabel>
                  <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    name="frequency"
                    className={"frequency"}
                    onChange={this.bindInput}
                  >
                    <MenuItem value={0}>OneOff</MenuItem>
                    <MenuItem value={1}>Weekly</MenuItem>
                    <MenuItem value={2}>Fortnightly</MenuItem>
                    <MenuItem value={3}>Monthly</MenuItem>
                  </Select>
                </td>
              </tr>
              <tr>
                <td colSpan="3">
                  <UserList
                    title="Contributors Pending"
                    listItems={this.props.flatMembers}
                    onListChange={this.updateContributorsPending}
                  />
                </td>
              </tr>
              <tr>
                <td colSpan="3">
                  <UserList
                    title="Contributors Paid"
                    listItems={this.props.flatMembers}
                    onListChange={this.updateContributorsPaid}
                  />
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
