import axios from "axios";
const URL = `http://localhost:5293/api/Chess/`;

async function getPlayersPerformance() {
  let data = null;

  try {
    let response = await axios.get(URL + "playerperformance");
    if (response.status === 200) {
      data = await response.data;
      console.log(data);
    }
  } catch (error) {
    return JSON.stringify(error);
  }
  return data;
}

async function getPlayersPerformanceAboveAverage() {
    let data = null;
  
    try {
      let response = await axios.get(URL + "GetPlayerWinPercentageByAverageOfWins");
      if (response.status === 200) {
        data = await response.data;
        console.log(data);
      }
    } catch (error) {
      return JSON.stringify(error);
    }
    return data;
}
async function getPlayersOfCountryByColumn(country,column) {
    let data = null;
  
    try {
      let response = await axios.get(URL + "GetPlayersOfCountryByColumn?country="+country+"&sortBy="+column);
      if (response.status === 200) {
        data = await response.data;
        console.log(data);
      }
    } catch (error) {
      return JSON.stringify(error);
    }
    return data;
  }

async function postMatch(match){
    let data = null;
    try{

      const matchData = {
        player1Id : match.player1,
        player2Id : match.player2,
        matchDate : match.date,
        matchLevel : match.level,
        winnerId : match.winner
      }

        fetch(URL + "AddNewMatch" ,{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(matchData)
        })
        .then((response) => response.json())
        .then((data) => {
            console.log(data);
            return data
        })
        .catch((error) => {
            console.log(error);
            return error
        })
    } catch (error) {
        return JSON.stringify(error);
    }
    return [data];
}

export {getPlayersPerformance,postMatch,getPlayersPerformanceAboveAverage,getPlayersOfCountryByColumn}