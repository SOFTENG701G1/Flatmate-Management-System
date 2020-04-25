import React from "react";
import "./ConfirmRemoveMember.css";

export default class Alert extends React.Component {
  render() {
    const showHideClassName = this.props.show
      ? "modal display-block"
      : "modal display-none";
    return (
      <div id="modal" className={showHideClassName}>
        <section className="modal-main">
          <p className="UserInformation" id="Name">
            Remove {""}
            {this.props.member.firstName} {this.props.member.lastName}
          </p>
          <p className="UserExtraInformation" id="Name">
            from the flat?
          </p>
          <button
            className="UserDetailsButtonDelete"
            onClick={this.props.delete}
          >
            DELETE
          </button>
          <button
            className="UserDetailsButtonCancel"
            onClick={this.props.handleClose}
          >
            CANCEL
          </button>
        </section>
      </div>
    );
  }
}
