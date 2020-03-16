import React from 'react';

export default class Members extends React.Component {
    constructor(props) {
        super(props);

        this.getMembers = this.getMembers.bind(this);
        this.getMembers();
    }

    render(){
        const showHideClassName = show ? "modal display-block" : "modal display-none";
        return (
            <div className={showHideClassname}>
              <section className="modal-main">
                <button id="editButton" onClick={this.props.handleClose}>close</button>
                <img src="../images/user-profile.png" />
                <h2 class="UserInformation" id="Name">Name Here</h2>
                <p class="InfoHeading">Username</p>
                <p class="UserInformation">Username Here</p>
                <p class="InfoHeading">Date of Birth</p>
                <p class="UserInformation">Username Here</p>
                <p class="InfoHeading">Phone</p>
                <p class="UserInformation">Username Here</p>
                <p class="InfoHeading">Email</p>
                <p class="UserInformation">Username Here</p>
                <p class="InfoHeading">Account Number</p>
                <p class="UserInformation">Username Here</p>
                <p class="InfoHeading">Medical Conditions</p>
                <p class="UserInformation">Username Here</p>
                
              </section>
            </div>
          );
    }
}