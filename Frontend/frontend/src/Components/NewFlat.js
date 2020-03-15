import React from 'react';
import './NewFlat.css';
import User from '../Util/User'
import { withRouter } from 'react-router-dom'
import { useHistory } from "react-router-dom";
import APIRequest from '../Util/APIRequest';
import Members from '../Views/Members';

export default class NewFlat extends React.Component {

  render() {

    return (

      <div>
        <h2 class="title">You are not currently part of a flat</h2>
        <p class="subheading">Press the "Create Flat" Button to create a new flat</p>
        <HomeButton />
      </div>
    );
  }

}


function HomeButton() {
  const history = useHistory();

  async function handleClick() {
    var resp = await APIRequest.createNewFlat();
    var flat = await resp.json();
    User.setFlatState(flat);
    if(resp.status == 201){
      history.push("/app/members");
    }  
  }

  return (
    <button type="button" onClick={handleClick}>
      Create Flat
    </button>
  );
}






