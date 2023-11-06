export const gameUserStatus = {
    Pending: {
      id: 0,
      text: "Запрос"
    },
    Approved: {
      id: 1,
      text: "Подтвержден"
    },
    Declined: {
      id: 2,
      text: "Запрещен"
    },
  }

export const gameStatus = {
    Opened: {
      id: 0,
      text: "Открытая",
      cssClass: "text-bg-success",
    },
    InProgress: {
      id: 1,
      text: "В процессе",
      cssClass: "text-bg-info",
    },
    CountingResults: {
      id: 2,
      text: "Подсчет очков",
      cssClass: "text-bg-warning",
    },
    Closed: {
      id: 3,
      text: "Закрытая",
      cssClass: "text-bg-secondary",
    },
  }

  export const gameTypes = {
    Public: {
        id: 0,
        text: "Открытая"
    },
    Private: {
        id: 1,
        text: "Закрытая"
    },
  }