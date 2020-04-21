import React from "react";
import UserImage from "../images/user-profile.png";
import UserDetails from "./UserDetails";
import Alert from "./ConfirmRemoveMember";
import Utils from "../Util/Utils";
import APIRequest from "../Util/APIRequest";

export default class MemberTile extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      person: props.flatMember,
      isUser: false,
      show: false,
      alert: false,
    };

    this.showModal = this.showModal.bind(this);
    this.hideModal = this.hideModal.bind(this);
    this.showAlert = this.showAlert.bind(this);
    this.hideAlert = this.hideAlert.bind(this);
    this.deleteUser = this.deleteUser.bind(this);
    this.refreshParent = this.refreshParent.bind(this);
  }

  showModal() {
    this.setState({ show: true });
  }

  showAlert() {
    this.setState({ alert: true });
  }

  hideModal() {
    this.setState({ show: false });
  }

  hideAlert() {
    this.setState({ alert: false });
  }
  refreshParent() {
    this.props.refresh();
  }

  async deleteUser() {
    const res = await APIRequest.deleteMember(this.state.person.userName);
    if (res.status === 200) {
      this.refreshParent();
    }
  }

  render() {
    var { person, isUser, show, alert } = this.state;
    return (
      <temp>
        <div className="MemberProfile">
          <button className="MemberDelete" onClick={this.showAlert}>
            X
          </button>
          <img
            alt="user-profile"
            className="userProfile"
            src={UserImage}
            onClick={this.showModal}
          />
          <p className="name">
            {" "}
            {person.firstName} {person.lastName} {isUser ? "(ME)" : ""}
          </p>
          <p className="info">
            {" "}
            Date of Birth: {Utils.dateFormatter(person.dateOfBirth)}{" "}
          </p>
          <p className="info"> Phone Number: {person.phoneNumber}</p>
        </div>
        <UserDetails show={show} handleClose={this.hideModal} member={person} />
        <Alert
          show={alert}
          handleClose={this.hideAlert}
          delete={this.deleteUser}
          member={person}
        />
      </temp>
    );
  }
}
