# LPContribMvc
LPContribMvc is MvcContrib's grid style in MVC Framework with Multi Heading / Multi-band feature

For example, this is the part of .cshtml of the grid declaration:

```
   @Html.Grid(Model).Columns(column =>
    {
					column.For(ent => "<span style='cursor: pointer; color: teal;' class='fa fa-chevron-right fa-lg'></span>").Named("His.").HeaderAttributes(@id => "$001").Attributes(@style => "width:20px").Encode(false);
					column.For(ent => "<input class='form-check-input' type='checkbox' value='' />").Named("Sel.").HeaderAttributes(@id => "$002").Attributes(@style => "width:15px").Encode(false);
     column.For(ent => ent.SentDate).Named("Incoming").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "SentDate");
     column.For(ent => ent.DateCreated).Named("Created").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "DateCreated");
					column.Add()
						.ChildColumns(a => {
								a.For(ent => ent.Sco).Named("Co").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "Sco");
								a.For(ent => ent.Sdept).Named("Dept").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "Sdept");
								a.For(ent => ent.FrDept).Named("Abv").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "FrDept");
							})
						.Named("From");
					column.Add()
						.ChildColumns(a => {
								a.For(ent => ent.Dco).Named("Co").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "Dco");
								a.Add()
								.ChildColumns(b => {
									b.For(ent => ent.Ddept).Named("Dept").Attributes(@style => "white-space:nowrap").HeaderAttributes(@id => "Ddept");
									b.For(ent => ent.ToDept).Named("Abv").Attributes(@style => "white-space:nowrap;").HeaderAttributes(@id => "ToDept");
								}).Named("Test");	
							})
						.Named("To");
     column.For(ent => ent.DocDesc).Named("Description").HeaderAttributes(@id => "DocDesc");
     column.For(ent => ent.TDocsUID).Named("UID").HeaderAttributes(@class => "UID", @id => "TDocsUID");
    }).Attributes(@class => "table table-striped table-bordered", @id => "datatable-paging", @style => "width:100%").RowAttributes(data => new LPContribMvc.Hash(@class => data.IsAlternate ? "even pointer w3-hover-grey" : "odd pointer w3-hover-grey"))
```

It will produce:


