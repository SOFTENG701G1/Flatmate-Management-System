import React from "react";
import "./Alert.css";

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
          <p className="UserInformation2" id="Name">
            from the flat?
          </p>
          <button className="userDetailsButton2" onClick={this.props.delete}>
            DELETE
          </button>
          <button
            className="userDetailsButton"
            onClick={this.props.handleClose}
          >
            CANCEL
          </button>
        </section>
      </div>
    );
  }
}
