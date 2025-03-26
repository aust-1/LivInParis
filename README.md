# LivinParis

## Projet d'architecture

LivinParis
├─ docs
│  └─ PSI.loo
├─ LivinParis.sln
├─ README.md
├─ src
│  ├─ database
│  │  ├─ .env
│  │  ├─ docker-compose.yml
│  │  └─ init.sql
│  └─ LivinParis
│     ├─ Controllers ???
│     ├─ data
│     │  ├─ metro
│     │  │  ├─ arcs.csv
│     │  │  ├─ correspondance.csv
│     │  │  └─ noeuds.csv
│     │  └─ Repositories
│     │     ├─ Implementations
│     │     │  ├─ AccountRepository.cs
│     │     │  ├─ AdressRepository.cs
│     │     │  ├─ ChefRepository.cs
│     │     │  ├─ CompanyRepository.cs
│     │     │  ├─ ContainsRepository.cs
│     │     │  ├─ CustomerRepository.cs
│     │     │  ├─ DishRepository.cs
│     │     │  ├─ IndividualRepository.cs
│     │     │  ├─ IngredientRepository.cs
│     │     │  ├─ MenuProposalRepository.cs
│     │     │  ├─ OrderLineRepository.cs
│     │     │  ├─ ReviewRepository.cs
│     │     │  └─ TransactionRepository.cs
│     │     └─ Interfaces
│     │        ├─ IAccountRepository.cs
│     │        ├─ IAdressRepository.cs
│     │        ├─ IChefRepository.cs
│     │        ├─ ICompanyRepository.cs
│     │        ├─ IContainsRepository.cs
│     │        ├─ ICustomerRepository.cs
│     │        ├─ IDishRepository.cs
│     │        ├─ IIndividualRepository.cs
│     │        ├─ IIngredientRepository.cs
│     │        ├─ IMenuProposalRepository.cs
│     │        ├─ IOrderLineRepository.cs
│     │        ├─ IReviewRepository.cs
│     │        └─ ITransactionRepository.cs
│     ├─ LivinParis.csproj
│     ├─ Models
│     │  ├─ Maps
│     │  │  ├─ Edge.cs
│     │  │  ├─ Graph.cs
│     │  │  ├─ Node.cs
│     │  │  └─ Station.cs
│     │  └─ Order
│     │     ├─ Account.cs
│     │     ├─ Adress.cs
│     │     ├─ Chef.cs
│     │     ├─ Company.cs
│     │     ├─ Contains.cs
│     │     ├─ Customer.cs
│     │     ├─ Dish.cs
│     │     ├─ Individual.cs
│     │     ├─ Ingredient.cs
│     │     ├─ MenuProposal.cs
│     │     ├─ OrderLine.cs
│     │     ├─ Review.cs
│     │     └─ Transaction.cs
│     ├─ Profiles ???
│     │  ├─ Chef.cs
│     │  ├─ Company.cs
│     │  ├─ Customer.cs
│     │  ├─ Individual.cs
│     │  └─ Manager.cs
│     ├─ Program.cs
│     └─ Using.cs
└─ tests
   └─ LivinParis.Tests
      ├─ EdgeTests.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs

{
  "jupyter.notebookFileRoot": "${workspaceFolder}",
  "workbench.settings.applyToAllProfiles": [
    "jupyter.notebookFileRoot"
  ],
  "git.autofetch": true,
  "editor.formatOnSave": true,
  "editor.autoIndent": "advanced",
  "editor.formatOnType": true,
  "editor.formatOnPaste": true,
  "[csharp]": {
    "editor.defaultFormatter": "csharpier.csharpier-vscode",
  },
  "[xml]": {
    "editor.defaultFormatter": "csharpier.csharpier-vscode"
  },
  "[python]": {
    "diffEditor.ignoreTrimWhitespace": false,
    "editor.defaultColorDecorators": "never",
    "editor.defaultFormatter": "ms-python.black-formatter",
    "gitlens.codeLens.symbolScopes": [
      "!Module"
    ],
    "editor.formatOnType": true,
    "editor.wordBasedSuggestions": "off"
  },
  "diffEditor.codeLens": true,
  "github.copilot.enable": {
    "markdown": true
  },
  "editor.fontFamily": "JetBrains Mono",
  "editor.fontLigatures": true,
  "terminal.integrated.fontFamily": "monospace",
  "workbench.colorCustomizations": {
    "editorBracketMatch.border": "#422240",
    "editorBracketMatch.background": "#422240",
    "editorBracketHighlight.unexpectedBracket.foreground": "#FF5647",
    "editorBracketHighlight.foreground1": "#BDBDBD",
    "editorBracketHighlight.foreground2": "#BDBDBD",
    "editorBracketHighlight.foreground3": "#BDBDBD",
  },
  "editor.semanticHighlighting.enabled": true,
  "editor.tokenColorCustomizations": {
    "textMateRules": [
      {
        "scope": [
          "entity.name.type",
          "entity.name.type.class",
          "entity.name.type.interface",
          "entity.name.type.parameter",
          "entity.name.namespace"
        ],
        "settings": {
          "foreground": "#C191FF"
        }
      },
      {
        "scope": [
          "entity.name.type.delegate",
          "entity.name.type.enum",
          "entity.name.type.struct"
        ],
        "settings": {
          "foreground": "#E1BFFF"
        }
      },
      {
        "scope": [
          "entity.name.variable.field",
          "entity.name.variable.static",
          "constant",
          "variable.other.property",
          "entity.name.variable.property"
        ],
        "settings": {
          "foreground": "#66C3CC"
        }
      },
      {
        "scope": [
          "variable.function",
          "entity.name.function"
        ],
        "settings": {
          "foreground": "#39CC9B"
        }
      },
      {
        "scope": [
          "comment.block",
          "comment.line"
        ],
        "settings": {
          "foreground": "#85C46C",
          "fontStyle": "italic"
        }
      },
      {
        "scope": [
          "comment.documentation.name",
          "comment.documentation.attribute.name",
          "comment.block.documentation entity.name.tag.localname",
          "comment.block.documentation entity.other.attribute-name.localname"
        ],
        "settings": {
          "foreground": "#487D34"
        }
      },
      {
        "scope": [
          "entity.name.variable",
          "variable.parameter",
          "variable.other.readwrite",
          "meta.preprocessor",
          "meta.preprocessor.string"
        ],
        "settings": {
          "foreground": "#BDBDBD"
        }
      },
      {
        "scope": [
          "entity.name.event",
          "constant.numeric"
        ],
        "settings": {
          "foreground": "#ED94C0"
        }
      },
      {
        "scope": [
          "keyword",
          "storage.type",
          "keyword.operator.expression",
          "keyword.control",
          "meta.preprocessor punctuation.separator"
        ],
        "settings": {
          "foreground": "#6C95EB"
        }
      },
      {
        "scope": "string",
        "settings": {
          "foreground": "#C9A26D"
        }
      },
      {
        "scope": "constant.character.escape",
        "settings": {
          "foreground": "#D688D4"
        }
      }
    ]
  },
  "[markdown]": {
    "editor.defaultFormatter": "DavidAnson.vscode-markdownlint"
  },
  "workbench.editorAssociations": {
    "*.copilotmd": "vscode.markdown.preview.editor",
    "*.loo": "default"
  },
  "todo-tree.highlights.customHighlight": {
    "TODO": {
      "gutterIcon": true,
      "type": "text-and-comment",
      "foreground": "cyan"
    },
    "FIXME": {
      "iconColour": "yellow",
      "gutterIcon": true,
      "type": "text-and-comment",
      "foreground": "orange"
    },
    "BUG": {
      "iconColour": "red",
      "gutterIcon": true,
      "type": "text",
      "foreground": "red"
    },
    "QUESTION": {
      "icon": "question",
      "iconColour": "purple",
      "gutterIcon": true,
      "type": "text-and-comment",
      "foreground": "purple"
    },
  },
  "todo-tree.highlights.backgroundColourScheme": [
    "red",
    "orange",
    "yellow",
    "green",
    "blue",
    "indigo",
    "violet",
    "cyan"
  ],
  "todo-tree.general.tags": [
    "BUG",
    "HACK",
    "FIXME",
    "TODO",
    "QUESTION",
    "[ ]",
    "[x]"
  ]
}
